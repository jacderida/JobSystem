using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	public interface ITenantConfig
	{
		ConnectionStringSettings GetConnectionString(string connectionName);
	}
}