using System.Web;
using JobSystem.Framework.Configuration;

namespace JobSystem.Mvc.Configuration
{
    public class HostRequestConfigDomainProvider : IHostNameProvider
    {
        public string GetHostName()
        {
            var currentRequest = HttpContext.Current.Request;
            if (currentRequest == null)
                throw new System.InvalidOperationException("Cannot get configuration domain outwith http request. HttpContext.Current.Request was null");
            var hostHeader = currentRequest.Headers["Host"];
            return hostHeader;
        }
    }
}