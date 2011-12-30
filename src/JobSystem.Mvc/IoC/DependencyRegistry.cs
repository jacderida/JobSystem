using System.Configuration;
using System.Web;
using Autofac;
using JobSystem.BusinessLogic.IoC;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel;
using JobSystem.Framework.Configuration;
using JobSystem.Framework.Messaging;
using JobSystem.Framework.Security;
using JobSystem.Mvc.Configuration;
using JobSystem.Mvc.Core;
using JobSystem.Queueing.Msmq;

namespace JobSystem.Mvc.IoC
{
	public static class DependencyRegistry
	{
		public static void RegisterAll(ContainerBuilder builder)
		{
			builder.Register(c => new HttpContextWrapper(HttpContext.Current))
				.As<HttpContextBase>();
			builder.Register<IQueueDispatcher<IMessage>>(c =>
				{
					return new MsmqQueueGateway<IMessage>(ConfigurationManager.AppSettings["QueuePath"], ConfigurationManager.AppSettings["FailedQueuePath"]);
				}).SingleInstance();
			builder.RegisterType<WebUserContext>().As<IUserContext>();
			builder.RegisterType<CryptographicService>().As<ICryptographicService>();
			builder.RegisterModule(new ServiceModule());
			builder.RegisterAssemblyTypes(typeof(UserAccountRepository).Assembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsImplementedInterfaces();
			builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
			builder.RegisterType<LocalAppConfig>().As<IAppConfig>().InstancePerLifetimeScope();
			builder.RegisterType<HostRequestConfigDomainProvider>().As<IConfigDomainProvider>();
		}
	}
}