using System;
using System.Web;
using Autofac;
using Autofac.Integration.Web;
using JobSystem.DataAccess.NHibernate;
using JobSystem.Framework.Configuration;

namespace JobSystem.WebUI.Configuration
{
    /// <summary>
    /// <para>
    /// <see cref="global::NHibernate.Connection.DriverConnectionProvider"/> implementation that provides a connection string
    /// based upon the context of the current http request.
    /// </para>
    /// <para>
    /// This implementation allows NHibernate to switch database connections based upon the current request context, therefore
    /// allowing a single application to servce multiple domains.
    /// </para>
    /// </summary>
    public class RequestContextConnectionProvider : ContextConnectionProviderBase
    {
        protected override ITenantConfig GetConfigForContext()
        {
            var containerProviderAccessor = HttpContext.Current.ApplicationInstance as IContainerProviderAccessor;
            if (containerProviderAccessor == null)
                throw new InvalidOperationException("Cannot get connection string from configuration, current application is not an IContainerProviderAccessor");
            var lifetimeScope = containerProviderAccessor.ContainerProvider.RequestLifetime;
            if (lifetimeScope == null)
            {
                // There is the possiblity that we are being called outwith a request, app startup for example. In this case
                // we use the parent application container.
                lifetimeScope = containerProviderAccessor.ContainerProvider.ApplicationContainer;
            }
            if (lifetimeScope == null)
                throw new System.InvalidOperationException("Failed to get configuration from request or application container.");
            var appConfig = lifetimeScope.Resolve<ITenantConfig>();
            return appConfig;
        }
    }
}