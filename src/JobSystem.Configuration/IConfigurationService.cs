namespace JobSystem.Configuration
{
	/// <summary>
	/// Service methods for accessing configuration values
	/// </summary>
	public interface IConfigurationService
	{
		/// <summary>
		/// Gets the configuration value for the specified host.
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		/// <param name="configItem">The request configuration item.</param>
		/// <returns></returns>
		string GetConfigurationValue(string hostname, string configItem);

		string GetConnectionString(string hostname, string connectionName);
	}
}