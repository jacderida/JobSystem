using System;
using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	public class LocalTenantConfig : ITenantConfig, IDisposable
	{
		private ConnectionStringSettingsCollection _connectionStrings = new ConnectionStringSettingsCollection();

		public LocalTenantConfig()
		{
			foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
				_connectionStrings.Add(connection);
		}

		public ConnectionStringSettings GetConnectionString(string connectionName)
		{
			var connectionString = ConfigurationManager.ConnectionStrings[connectionName];
			if (connectionString == null)
				throw new ConfigurationErrorsException(
					String.Format("The connection string for '{0}' is missing from the connectionStrings section of the current app.config or web.config file.", connectionName));
			return connectionString;
		}

		public void Dispose()
		{
			// Required by Autofac.
		}
	}
}