namespace JobSystem.Configuration
{
	/// <summary>
	/// Service methods for accessing configuration values for tennants.
	/// </summary>
	public class ConfigurationService : IConfigurationService
	{
		#region Fields

		private readonly IConfigurationRepository _configurationRepository;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationService"/> class.
		/// </summary>
		/// <param name="repository">The repository that provides access to the data store for the configuration values.</param>
		public ConfigurationService(IConfigurationRepository repository)
		{
			_configurationRepository = repository;
		}

		#endregion

		#region Implementation of IConfigurationService

		/// <summary>
		/// <para>Gets the configuration value for the specified host.</para>
		/// <para>If the configuration item doesn't exist in the specified hostname configurations then we fall back on the common configuration items.
		/// This allows us to only specify the configuration items that we intend to be different for each tennant in the system. </para>
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		/// <param name="configItem">The request configuration item.</param>
		/// <exception cref="ConfigurationItemNotFoundException">Thrown if the configuration item is not found.</exception>
		/// <exception cref="HostConfigurationNotFoundException">Thrown if the hostname specified doesn't have a configuration defined.</exception>
		/// <returns></returns>
		public string GetConfigurationValue(string hostname, string configItem)
		{
			//check a configuration exists for a host, we will not fall back on the common settings if there are no individual settings for a host
			//as we are assuming that this means the host is not valid host.
			if (_configurationRepository.ConfigurationExists(hostname))
			{
				//fetch the configuration value from the host configuration or the common configuration.
				var result = _configurationRepository.GetConfigItem(hostname, configItem) ?? _configurationRepository.GetConfigItem(ServiceConstants.CommonHostName, configItem);

				if (result == null)
				{
					throw new ConfigurationItemNotFoundException(hostname, configItem);
				}

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

		#endregion
	}
}