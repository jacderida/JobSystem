namespace JobSystem.Configuration
{
	/// <summary>
	/// Service methods for accessing configuration values for tennants.
	/// </summary>
	public class ConfigurationService : IConfigurationService
	{
		private readonly IConfigurationRepository _configurationRepository;

		public ConfigurationService(IConfigurationRepository repository)
		{
			_configurationRepository = repository;
		}

		public string GetConfigurationValue(string hostname, string configItem)
		{
			if (_configurationRepository.ConfigurationExists(hostname))
			{
				var result = _configurationRepository.GetConfigItem(hostname, configItem) ?? _configurationRepository.GetConfigItem(ServiceConstants.CommonHostName, configItem);
				if (result == null)
					throw new ConfigurationItemNotFoundException(hostname, configItem);
				return result;
			}
			throw new HostConfigurationNotFoundException(hostname);
		}

		public string GetConnectionString(string hostName, string connectionStringName)
		{
			if (_configurationRepository.ConfigurationExists(hostName))
			{
				var result =
					_configurationRepository.GetConfigItem(hostName, connectionStringName) ??
						_configurationRepository.GetConfigItem(ServiceConstants.CommonHostName, connectionStringName);
				if (result == null)
					throw new ConnectionStringNotFoundException(hostName, connectionStringName);
				return result;
			}
			throw new HostConfigurationNotFoundException(hostName);
		}
	}
}