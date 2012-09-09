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
using JobSystem.Storage.Jobs;
using JobSystem.Storage.Providers.S3;
using JobSystem.Storage.Providers.SimpleDb;
using JobSystem.Storage.Providers.FileSystem;

namespace JobSystem.Mvc.IoC
{
	public static class DependencyRegistry
	{
		public static void RegisterAll(ContainerBuilder builder)
		{
			builder.Register(c => new HttpContextWrapper(HttpContext.Current))
				.As<HttpContextBase>();
			builder.RegisterType<NullQueueDispatcher>().As<IQueueDispatcher<IMessage>>();
			builder.RegisterType<WebUserContext>().As<IUserContext>();
			builder.RegisterType<CryptographicService>().As<ICryptographicService>();
			builder.RegisterModule(new ServiceModule());
			builder.RegisterAssemblyTypes(typeof(UserAccountRepository).Assembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
			builder.RegisterType<HostRequestConfigDomainProvider>().As<IHostNameProvider>();
			builder.RegisterType<S3JobAttachmentDataRepository>().As<IJobAttachmentDataRepository>();
			builder.RegisterType<JobAttachmentService>().AsSelf();
			RegisterTenantConfig(builder);
			RegisterAttachmentRepositories(builder);
		}

		private static void RegisterTenantConfig(ContainerBuilder builder)
		{
			if (Config.UseRemoteConfig())
				builder.RegisterType<RemoteTenantConfig>().As<ITenantConfig>().InstancePerLifetimeScope();
			else
				builder.RegisterType<LocalTenantConfig>().As<ITenantConfig>().InstancePerLifetimeScope();
		}

		private static void RegisterAttachmentRepositories(ContainerBuilder builder)
		{
			if (Config.UseRemoteConfig())
				builder.RegisterType<S3JobAttachmentDataRepository>().As<IJobAttachmentDataRepository>();
			else
				builder.RegisterType<FileSystemAttachmentStorage>().As<IJobAttachmentDataRepository>();
		}
	}
}