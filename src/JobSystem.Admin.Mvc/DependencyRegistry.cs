using Autofac;
using JobSystem.Admin.Mvc.Data;
using JobSystem.Admin.Mvc.Data.Sql;
using JobSystem.Framework.Security;

namespace JobSystem.Admin.Mvc
{
	public static class DependencyRegistry
	{
		public static void RegisterAll(ContainerBuilder builder)
		{
			builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
			builder.RegisterType<SqlUserAccountRepository>().As<IUserAccountRepository>();
			builder.RegisterType<SqlTenantRepository>().As<ITenantRepository>();
			builder.RegisterType<CryptographicService>().As<ICryptographicService>();
		}
	}
}