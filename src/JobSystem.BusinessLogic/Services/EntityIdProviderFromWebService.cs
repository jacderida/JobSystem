using System.Collections.Generic;
using System.IO;
using System.Net;
using JobSystem.DataModel;
using JobSystem.Framework.Configuration;

namespace JobSystem.BusinessLogic.Services
{
	public class EntityIdProviderFromWebService : IEntityIdProvider
	{
		private IConfigDomainProvider _configDomainProvider;
		private static readonly List<string> _localDomains = new List<string>()
		{
			"localhost",
			"8ry8v4j"
		};

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
			if (_localDomains.Contains(lower))
				return "http://" + lower + "/JobSystemMvc/";
			return "https://" + lower + "/";
		}
	}
}