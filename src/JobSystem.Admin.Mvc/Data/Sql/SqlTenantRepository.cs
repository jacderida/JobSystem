using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace JobSystem.Admin.Mvc.Data.Sql
{
	public class SqlTenantRepository : ITenantRepository
	{
		private const string GetTenantsQuery = "SELECT * FROM Tenants";

		public List<Tuple<Guid, string>> GetTenants()
		{
			var conn = GetConnection();
			try
			{
				conn.Open();
				var cmd = conn.CreateCommand();
				cmd.CommandText = GetTenantsQuery;
				var result = new List<Tuple<Guid, string>>();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						result.Add(new Tuple<Guid,string>(reader.GetGuid(0), reader.GetString(1)));
				}
				return result;
			}
			finally
			{
				conn.Close();
			}
		}

		private SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["TenantList"].ConnectionString);
		}
	}
}