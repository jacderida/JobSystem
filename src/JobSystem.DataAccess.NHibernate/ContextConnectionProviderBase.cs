using System.Data.SqlClient;
using JobSystem.Framework.Configuration;
using global::NHibernate.Connection;

namespace JobSystem.DataAccess.NHibernate
{
	/// <summary>
	/// <para>
	/// <see cref="DriverConnectionProvider"/> implementation that provides a connection string based upon the context of
	/// the current request.
	/// </para>
	/// </summary>
	public abstract class ContextConnectionProviderBase : DriverConnectionProvider
	{
		#region Properties

		protected override string ConnectionString
		{
			get
			{
				var appConfig = GetConfigForContext();
				var csb = new SqlConnectionStringBuilder();
				csb.DataSource = appConfig.DatabaseServer;
				csb.InitialCatalog = appConfig.DatabaseCatalogName;
				csb.UserID = appConfig.DatabaseUsername;
				csb.Password = appConfig.DatabasePassword;
				return csb.ToString();
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets an <see cref="IAppConfig"/>, from which connection string details can be read, for the calling context.
		/// </summary>
		/// <returns><see cref="IAppConfig"/> for the calling context</returns>
		protected abstract IAppConfig GetConfigForContext();

		#endregion Methods
	}
}