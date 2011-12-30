using Autofac;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;

namespace JobSystem.BusinessLogic.IoC
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
				.Where(t => t.BaseType == typeof(ServiceBase))
				.AsSelf()
				.InstancePerLifetimeScope();
			builder.RegisterType<EntityIdProviderFromWebService>().As<IEntityIdProvider>();
		}
	}
}