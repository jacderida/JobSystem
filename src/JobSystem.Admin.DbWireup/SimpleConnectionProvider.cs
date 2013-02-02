using NHibernate.Connection;
using System.Configuration;
using System.Data.SqlClient;

namespace JobSystem.Admin.DbWireup
{
    public class SimpleConnectionProvider : DriverConnectionProvider
    {
        public static string CatalogName { get; set; }

        protected override string ConnectionString
        {
            get
            {
                var csb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["JobSystem"].ConnectionString);
                csb.InitialCatalog = CatalogName;
                return csb.ToString();
            }
        }
    }
}