using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	public class RemoteTenantConfig : ITenantConfig
	{
		private readonly IConfigurationService _configurationService;
		private readonly IHostNameProvider _hostNameProvider;

		public RemoteTenantConfig(IConfigurationService configService, IHostNameProvider hostNameProvider)
		{
			_configurationService = configService;
			_hostNameProvider = hostNameProvider;
		}

		public ConnectionStringSettings GetConnectionString(string connectionName)
		{
			var hostName = _hostNameProvider.GetHostName();
			return _configurationService.GetConnectionString(hostName, connectionName);
		}
	}
}