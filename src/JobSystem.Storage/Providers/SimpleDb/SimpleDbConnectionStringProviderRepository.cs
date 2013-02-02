using System;
using System.Collections.Generic;
using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using JobSystem.Framework.Configuration;

namespace JobSystem.Storage.Providers.SimpleDb
{
    public class SimpleDbConnectionStringProviderRepository : IConnectionStringProviderRepository
    {
        private const string ConfigurationDomainName = "JobSystemConfigurations";
        private AmazonSimpleDB _db;
        private AmazonSimpleDB SimpleDb
        {
            get { return _db ?? (_db = GetSimpleDbClient()); }
        }

        /* If the requested hostname is non-existant, get the connection string for the health check.
         * 
         * This will be the situation when an arbitrary string is entered for the tenant name. In that case, we just revert
         * to the health check database, and this will require the user to login. Their login will fail, as there will
         * be no users existing in that database.
         * 
         * It can also occur after the IIS application pool recycles. After the app pool recycles, the first request that comes in
         * causes the NHibernate session factory to be rebuilt. It's quite possible that the first request after recycle can
         * be the load balancer health check, which will be a request with an IP address as the host name, and there's obviously
         * no connection string for that. Hence, use the health check database.
         * */
        public string Get(string hostname)
        {
            var result = SimpleDb.GetAttributes(new GetAttributesRequest().WithDomainName(ConfigurationDomainName).WithItemName(hostname));
            if (result.GetAttributesResult.Attribute.Count == 0)
                return GetHealthCheckConnectionString();
            var item = result.GetAttributesResult.Attribute[1];
            return item.Value;
        }

        public void Put(string hostname, string connectionString)
        {
            var putRequest = new PutAttributesRequest().WithDomainName(ConfigurationDomainName).WithItemName(hostname);
            var connAttributes = putRequest.Attribute;
            connAttributes.Add(new ReplaceableAttribute().WithName("Name").WithValue(hostname));
            connAttributes.Add(new ReplaceableAttribute().WithName("ConnectionString").WithValue(connectionString));
            SimpleDb.PutAttributes(putRequest);
        }

        private AmazonSimpleDB GetSimpleDbClient()
        {
            var accessKey = Config.AwsKey;
            var privateKey = Config.AwsSecretKey;
            return AWSClientFactory.CreateAmazonSimpleDBClient(
                accessKey, privateKey, new AmazonSimpleDBConfig() { ServiceURL = "https://sdb.eu-west-1.amazonaws.com" });
        }

        private string GetHealthCheckConnectionString()
        {
            var result = SimpleDb.GetAttributes(new GetAttributesRequest().WithDomainName(ConfigurationDomainName).WithItemName("healthcheck"));
            return result.GetAttributesResult.Attribute[1].Value;
        }
    }
}