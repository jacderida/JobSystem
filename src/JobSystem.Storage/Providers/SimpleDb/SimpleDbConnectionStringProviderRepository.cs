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

		public string Get(string hostname)
		{
			var result = SimpleDb.GetAttributes(new GetAttributesRequest().WithDomainName(ConfigurationDomainName).WithItemName(hostname));
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
	}
}