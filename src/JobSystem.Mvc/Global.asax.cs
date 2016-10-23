using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Web;
using FluentNHibernate.Cfg.Db;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Mappings;
using JobSystem.Mvc.IoC;
using JobSystem.WebUI.Configuration;

namespace JobSystem.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication, IContainerProviderAccessor
    {
        private WebSessionStorage _webSessionStorage;
        private static IContainerProvider _containerProvider;
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        public override void Init()
        {
            _webSessionStorage = new WebSessionStorage(this);
            base.Init();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(InitialiseNHibernateSessions);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            DependencyRegistry.RegisterAll(builder);
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule(new AutofacWebTypesModule());
            var container = builder.Build();
            _containerProvider = new ContainerProvider(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);    
        }

        private void InitialiseNHibernateSessions()
        {
            var connectionString = NHibernateSession.GetInitialConnectionString();
            using (var writer = System.IO.File.AppendText(@"c:\temp\log"))
            {
                writer.WriteLine("Retrieved connection string: " + connectionString);
            }
            var dbConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString).Provider<RequestContextConnectionProvider>();
            NHibernateSession.Init(
                _webSessionStorage, new[] { Server.MapPath("~/bin/JobSystem.DataAccess.NHibernate.dll") }, new AutoPersistenceModelGenerator().Generate(), null, null, null, dbConfigurer);
        }
    }
}