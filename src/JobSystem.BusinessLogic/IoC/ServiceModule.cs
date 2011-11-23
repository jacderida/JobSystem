using Autofac;
using JobSystem.BusinessLogic.Services;

namespace JobSystem.BusinessLogic.IoC
{
	/// <summary>
	/// Module containing the registrations for the Service implemenations
	/// </summary>
	public class ServiceModule : Module
	{
		#region Methods

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
				.Where(t => t.BaseType == typeof(ServiceBase))
				.AsSelf()
				.InstancePerLifetimeScope();
		}

		#endregion Methods
	}
}