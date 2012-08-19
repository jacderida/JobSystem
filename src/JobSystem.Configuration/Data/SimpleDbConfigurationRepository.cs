// <copyright company="Gael Limited">
// Copyright (c) 2011 All Right Reserved
// </copyright>
// <summary>
//	Complying with all copyright laws is the responsibility of the 
//	user. Without limiting rights under copyrights, neither the 
//	whole nor any part of this document may be reproduced, stored 
//	in or introduced into a retrieval system, or transmitted in any 
//	form or by any means (electronic, mechanical, photocopying, 
//	recording, or otherwise), or for any purpose without express 
//	written permission of Gael Limited.
// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using JobSystem.Configuration;
using JobSystem.Framework.Configuration;
using Attribute = Amazon.SimpleDB.Model.Attribute;

namespace Saturn.Configuration.Service.Data
{
	public class SimpleDbConfigurationRepository : IConfigurationRepository
	{

		private const int CacheExpiryInMinutes = 5;
		private const string ConfigurationDomainName = "SaturnConfigurations";
		private AmazonSimpleDB _db;

		private AmazonSimpleDB SimpleDb
		{
			get { return _db ?? (_db = GetSimpleDbClient()); }
		}

		public List<string> GetHostList()
		{
			var result = SimpleDb.Select(new SelectRequest() { SelectExpression = String.Format("Select * From `{0}`", ConfigurationDomainName) });
			return result.SelectResult.Item.Select(i => i.Name).ToList();
		}
		public Dictionary<string, string> GetHostConfigItems(string hostname)
		{
			var attributes = GetAttributesFromSimpleDb(hostname);
			return attributes.ToDictionary(a => a.Name, a => a.Value);
		}

		public string GetConfigItem(string hostname, string configItem)
		{
			return GetFromSimpleDb(hostname, configItem);
		}

		public bool ConfigurationExists(string hostname)
		{
			return SimpleDbContains(hostname);
		}

		public void Put(string hostname, string itemName, string value)
		{
			if (String.IsNullOrEmpty(hostname))
				throw new ArgumentException("A hostname must be provided");
			if (String.IsNullOrEmpty(itemName))
				throw new ArgumentException("The item name must be provided");
			if (String.IsNullOrEmpty(value))
				throw new ArgumentException("A value must be provided for the config item");
			if (value.Length > 255)
				throw new ArgumentException("The value cannot exceed 255 characters");
			var putRequest = new PutAttributesRequest()
			{
				DomainName = ConfigurationDomainName,
				ItemName = hostname,
				Attribute = new List<ReplaceableAttribute>() { new ReplaceableAttribute { Name = itemName, Value = value, Replace = true } }
			};
			SimpleDb.PutAttributes(putRequest);
		}

		public void PutHostConfig(string hostname, Dictionary<string, string> configuration)
		{
			var putRequest = new PutAttributesRequest()
			{
				DomainName = ConfigurationDomainName,
				ItemName = hostname,
				Attribute = new List<ReplaceableAttribute>(configuration.Select(c => new ReplaceableAttribute() { Name = c.Key, Value = c.Value, Replace = true }))
			};
			SimpleDb.PutAttributes(putRequest);
		}

		private string GetFromSimpleDb(string hostname, string configItem)
		{
			var item = GetAttributesFromSimpleDb(hostname);
			return item == null ? null : GetValueFromListOfAttributes(item, configItem);
		}

		private bool SimpleDbContains(string hostname)
		{
			return GetAttributesFromSimpleDb(hostname) != null;
		}

		private IList<Attribute> GetAttributesFromSimpleDb(string hostname)
		{
			var result = SimpleDb.GetAttributes(new GetAttributesRequest().WithDomainName(ConfigurationDomainName).WithItemName(hostname));
			var item = result.GetAttributesResult.Attribute;
			return item.Count > 0 ? item : null;
		}

		private static string GetValueFromListOfAttributes(IEnumerable<Attribute> item, string configItem)
		{
			var attribute = item.SingleOrDefault(att => att.Name.Equals(configItem));
			return attribute == null ? null : attribute.Value;
		}

		private AmazonSimpleDB GetSimpleDbClient()
		{
			var accessKey = Config.AwsKey;
			var privateKey = Config.AwsSecretKey;
			return AWSClientFactory.CreateAmazonSimpleDBClient(accessKey, privateKey, new AmazonSimpleDBConfig() { ServiceURL = "https://sdb.eu-west-1.amazonaws.com" });
		}
	}
}