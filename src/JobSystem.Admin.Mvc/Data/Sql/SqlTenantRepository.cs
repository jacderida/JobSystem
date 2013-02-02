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
        private const string TenantNameExistsQuery = "SELECT COUNT(*) FROM Tenants WHERE TenantName = @Name";
        private const string GetTenantsQuery = "SELECT * FROM Tenants";
        private const string InsertTenantQuery = "INSERT INTO Tenants(Id, TenantName, CompanyName) VALUES(@Id, @TenantName, @CompanyName)";
        private const string InsertTenantConnectionStringQuery = "INSERT INTO TenantConnectionStrings(Name, ConnectionString) VALUES(@Name, @ConnectionString)";

        public bool TenantNameExists(string tenantName)
        {
            var conn = GetConnection();
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = TenantNameExistsQuery;
                cmd.Parameters.Add(new SqlParameter("@Name", tenantName));
                return (int)cmd.ExecuteScalar() == 1;
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertTenant(Guid id, string tenantName, string companyName)
        {
            var conn = GetConnection();
            try
            {
                conn.Open();
                var cmd = GetInsertTenantCommand(id, tenantName, companyName, conn);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public void InsertTenantConnectionString(string tenantName, string connectionString)
        {
            var conn = GetConnection();
            try
            {
                conn.Open();
                var cmd = GetInsertTenatnConnectionStringCommand(tenantName, connectionString, conn);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Tuple<Guid, string, string>> GetTenants()
        {
            var conn = GetConnection();
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = GetTenantsQuery;
                var result = new List<Tuple<Guid, string, string>>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(new Tuple<Guid, string, string>(reader.GetGuid(0), reader.GetString(1), reader.GetString(2)));
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

        private SqlCommand GetInsertTenantCommand(Guid id, string tenantName, string companyName, SqlConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = InsertTenantQuery;
            cmd.Parameters.Add(new SqlParameter("@Id", id));
            cmd.Parameters.Add(new SqlParameter("@TenantName", tenantName));
            cmd.Parameters.Add(new SqlParameter("@CompanyName", companyName));
            return cmd;
        }

        private SqlCommand GetInsertTenatnConnectionStringCommand(string tenantName, string connectionString, SqlConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = InsertTenantConnectionStringQuery;
            cmd.Parameters.Add(new SqlParameter("@Name", tenantName));
            cmd.Parameters.Add(new SqlParameter("@ConnectionString", connectionString));
            return cmd;
        }
    }
}