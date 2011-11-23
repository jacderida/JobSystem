namespace JobSystem.Configuration
{
	/// <summary>
	/// Interface for the repository to provid access to the configuration data store.
	/// </summary>
	public interface IConfigurationRepository
	{
		/// <summary>
		/// Gets the configuration value for the specified host from the data store.
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		/// <param name="configItem">The request configuration item.</param>
		string GetConfigItem(string hostname, string configItem);


		/// <summary>
		/// Determines whether the host has a configuration defined.
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		/// <returns>
		/// 	<c>true</c> if the host has an existing configuration; otherwise, <c>false</c>.
		/// </returns>
		bool ConfigurationExists(string hostname);
	}
}