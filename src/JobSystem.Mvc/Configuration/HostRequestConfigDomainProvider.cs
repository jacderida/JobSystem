namespace JobSystem.Mvc.Configuration
{
	using System.Web;
	using JobSystem.Framework.Configuration;

	/// <summary>
	/// <see cref="IConfigDomainProvider"/> implementation that uses the host header of the current HTTP request
	/// as the configuration domain.
	/// </summary>
	public class HostRequestConfigDomainProvider : IConfigDomainProvider
	{
		#region Methods

		/// <summary>
		/// Gets the configuration domain.
		/// </summary>
		/// <returns>current configuration domain</returns>
		public string GetConfigDomain()
		{
			var currentRequest = HttpContext.Current.Request;
			if (currentRequest == null)
				throw new System.InvalidOperationException("Cannot get configuration domain outwith http request. HttpContext.Current.Request was null");
			var hostHeader = currentRequest.Headers["Host"];
			return hostHeader;
		}

		#endregion Methods
	}
}