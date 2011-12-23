using NHibernate.Connection;
using System.Configuration;
using System.Data.SqlClient;

namespace JobSystem.DbWireup
{
	public class SimpleConnectionProvider : DriverConnectionProvider
	{
		public static string CatalogName { get; set; }

		protected override string ConnectionString
		{
			get
			{
				var csb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["JobSystemDatabase"].ConnectionString);
				csb.InitialCatalog = CatalogName;
				return csb.ToString();
			}
		}
	}
}