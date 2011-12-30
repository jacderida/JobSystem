using System.Collections.Generic;
using System.IO;
using System.Net;
using JobSystem.DataModel;
using JobSystem.Framework.Configuration;
using System;

namespace JobSystem.BusinessLogic.Services
{
	public class EntityIdProviderFromWebService : IEntityIdProvider
	{
		private IConfigDomainProvider _configDomainProvider;

		public EntityIdProviderFromWebService(IConfigDomainProvider configDomainProvider)
		{
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
			var lower = domain.ToLowerInvariant();
			if (lower.Contains("localhost:"))
				return String.Format("http://{0}/", lower);
			else if (lower == "localhost")
				return String.Format("http://{0}/JobSystemMvc/", lower);
			return "https://" + lower + "/";
		}
	}
}