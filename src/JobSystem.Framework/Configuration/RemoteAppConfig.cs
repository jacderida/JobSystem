using System.ComponentModel;
using JobSystem.Configuration;

namespace JobSystem.Framework.Configuration
{
	/// <summary>
	/// <see cref="IAppConfig"/> implementation that uses an <see cref="IConfigurationService"/> to resolve configuration
	/// values. <see cref="IConfigurationService"/> may be a remote service, or a local filesystem service.
	/// </summary>
	public class RemoteAppConfig : IAppConfig
	{
		#region Fields

		private readonly IConfigurationService _configurationService;
		private readonly IHostNameProvider _hostNameProvider;

		#endregion

		#region Constructors

		public RemoteAppConfig(IConfigurationService configService, IHostNameProvider hostNameProvider)
		{
			_configurationService = configService;
			_hostNameProvider = hostNameProvider;
		}

		#endregion

		#region Methods

		private T GetConfigValue<T>(string itemName)
		{
			var configValue = _configurationService.GetConfigurationValue(_hostNameProvider.GetHostName(), itemName);
			var convertor = TypeDescriptor.GetConverter(typeof(T));

			// make sure that we actually have a method for converting to the type the user has specified.
			if (convertor == null)
			{
				throw new ConfigurationConversionException(typeof(T));
			}

			return (T)convertor.ConvertFrom(configValue);
		}

		private string GetConnectionString(string connectionName)
		{
			var hostName = _hostNameProvider.GetHostName();
			var connectionString = _configurationService.GetConnectionString(hostName, connectionName);
			if (connectionString == null)
				throw new ConnectionStringNotFoundException(hostName, connectionName);
			return connectionString;
		}

		#endregion

		#region Implementation of IAppConfig
		#region AppSettings

		/// <summary>
		/// Gets the Amazon public key
		/// </summary>
		public string AmazonKey
		{
			get { return GetConfigValue<string>("AWSK"); }
		}

		/// <summary>
		/// Gets the Amazon private key
		/// </summary>
		public string AmazonPrivateKey
		{
			get { return GetConfigValue<string>("AWSPK"); }
		}

		/// <summary>
		/// Gets name of the bucket that contains the serialised view models.
		/// </summary>
		/// <value>The view model bucket.</value>
		public string ViewModelBucket
		{
			get { return GetConfigValue<string>("ViewModelBucket"); }
		}

		/// <summary>
		/// Gets the database catalog name
		/// </summary>
		public string DatabaseCatalogName
		{
			get { return GetConfigValue<string>("DatabaseCatalogName"); }
		}

		/// <summary>
		/// Gets the database password for <see cref="IAppConfig.DatabaseUsername"/>
		/// </summary>
		public string DatabasePassword
		{
			get { return GetConfigValue<string>("DatabasePassword"); }
		}

		/// <summary>
		/// Gets the database server name
		/// </summary>
		public string DatabaseServer
		{
			get { return GetConfigValue<string>("DatabaseServer"); }
		}

		/// <summary>
		/// Gets the Risk database username
		/// </summary>
		public string DatabaseUsername
		{
			get { return GetConfigValue<string>("DatabaseUsername"); }
		}

		#endregion
		#endregion
	}
}