using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	public class RemoteTenantConfig : ITenantConfig
	{
		private readonly IConnectionStringProviderRepository _connectionStringProvider;
		private readonly IHostNameProvider _hostNameProvider;

		public RemoteTenantConfig(IConnectionStringProviderRepository connectionStringProvider, IHostNameProvider hostNameProvider)
		{
			_connectionStringProvider = connectionStringProvider;
			_hostNameProvider = hostNameProvider;
		}

		public ConnectionStringSettings GetConnectionString(string connectionName)
		{
			var hostName = _hostNameProvider.GetHostName();
			var connectionString = _connectionStringProvider.Get(hostName);
			return new ConnectionStringSettings(hostName, connectionString);
		}
	}
}