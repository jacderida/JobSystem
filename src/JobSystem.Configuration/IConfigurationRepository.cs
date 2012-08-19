using System.Collections.Generic;

namespace JobSystem.Configuration
{
	public interface IConfigurationRepository
	{
		List<string> GetHostList();
		Dictionary<string, string> GetHostConfigItems(string hostname);
		string GetConfigItem(string hostname, string configItem);
		bool ConfigurationExists(string hostname);
		void Put(string hostname, string itemName, string value);
		void PutHostConfig(string hostname, Dictionary<string, string> configuration);
	}
}