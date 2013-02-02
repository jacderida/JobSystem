using global::NHibernate.Connection;
using JobSystem.Framework.Configuration;

namespace JobSystem.DataAccess.NHibernate
{
    public abstract class ContextConnectionProviderBase : DriverConnectionProvider
    {
        protected override string ConnectionString
        {
            get
            {
                var tenantConfig = GetConfigForContext();
                return tenantConfig.GetConnectionString("JobSystem").ConnectionString;
            }
        }

        protected abstract ITenantConfig GetConfigForContext();
    }
}