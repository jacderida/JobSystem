namespace JobSystem.Configuration
{
	public interface IConfigurationRepository
	{
		string GetConfigItem(string hostname, string configItem);
		bool ConfigurationExists(string hostname);
	}
}