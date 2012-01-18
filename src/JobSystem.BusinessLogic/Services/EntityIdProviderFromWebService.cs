using System.Collections.Generic;
using System.IO;
using System.Net;
using JobSystem.DataModel;
using JobSystem.Framework.Configuration;
using System;
using System.Configuration;

namespace JobSystem.BusinessLogic.Services
{
	public class EntityIdProviderFromWebService : IEntityIdProvider
	{
		private IAppConfig _appConfig;
		private IConfigDomainProvider _configDomainProvider;

		public EntityIdProviderFromWebService(IAppConfig appConfig, IConfigDomainProvider configDomainProvider)
		{
			_appConfig = appConfig;
			_configDomainProvider = configDomainProvider;
		}

		public string GetIdFor<T>()
		{
			var typeName = typeof(T).ToString();
			var domain = _configDomainProvider.GetConfigDomain();
			var getIdUrl = GetUrlForDomain(domain);
			getIdUrl = getIdUrl + @"EntityId/GetId/" + typeName;

			var webRequest = (HttpWebRequest)WebRequest.Create(getIdUrl);
			string entityId;
			using (var response = webRequest.GetResponse())
			{
				using (var stream = response.GetResponseStream())
				{
					var streamReader = new StreamReader(stream);
					entityId = streamReader.ReadToEnd();
				}
			}
			return entityId;
		}

		protected virtual string GetUrlForDomain(string domain)
		{
			var protocol = _appConfig.UseHttps ? "https://" : "http://";
			var lower = domain.ToLowerInvariant();
			if (lower.Contains("localhost:"))
				return String.Format("{0}{1}/", protocol, lower);
			else if (lower == "localhost")
				return String.Format("{0}{1}/JobSystem.Mvc/", lower);
			return String.Format("{0}{1}/JobSystem.Mvc/", protocol, lower);		// TODO: REMOVE THIS!
		}
	}
}