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
			var tenantName = GetTenantName(hostName);
			var connectionString = _connectionStringProvider.Get(tenantName);
			return new ConnectionStringSettings(tenantName, connectionString);
		}

		private string GetTenantName(string hostname)
		{
			if (hostname == "localhost")
				return hostname;
			var index = hostname.IndexOf('.');
			return new string(hostname.ToCharArray(), 0, index);
		}
	}
}