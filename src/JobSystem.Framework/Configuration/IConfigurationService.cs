using System.Collections.Generic;
using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	public interface IConfigurationService
	{
		List<string> GetHostList();
		Dictionary<string, string> GetHostConfiguration(string hostname);
		string GetConfigurationValue(string hostname, string configItem);
		ConnectionStringSettings GetConnectionString(string hostName, string connectionName);
		void Put(string hostname, string itemName, string value);
		void PutConnectionString(string hostname, ConnectionStringSettings connectionStringSettings);
		void PutHostConfig(string hostname, Dictionary<string, string> configuration);
	}
}