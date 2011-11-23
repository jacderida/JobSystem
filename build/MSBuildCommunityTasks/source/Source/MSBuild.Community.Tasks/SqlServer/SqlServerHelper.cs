using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MSBuild.Community.Tasks.SqlServer
{
	public static class SqlServerHelper
	{
		private const string ExistingLoginQuery = "select COUNT(*) from master..syslogins WHERE name = '{0}'";
		private const string DatabaseExistsQuery = "USE master; SELECT COUNT(*) FROM sysdatabases WHERE name='{0}'";
		public static readonly List<string> ValidDatabaseLevelRoles =
			new List<string>
			{
				"db_owner",
				"db_securityadmin",
				"db_accessadmin",
				"db_backupoperator",
				"db_ddladmin",
				"db_datawriter",
				"db_datareader",
				"db_denydatawriter",
				"db_denydatareader"
			};


		public static SqlConnection GetDatabaseConnection(string databaseServer, string databaseUserName, string databasePassword)
		{
			return DoGetDatabaseConnection(databaseServer, "master", databaseUserName, databasePassword);
		}

		public static SqlConnection GetDatabaseConnection(string databaseServer, string databaseName, string databaseUserName, string databasePassword)
		{
			return DoGetDatabaseConnection(databaseServer, databaseName, databaseUserName, databasePassword);
		}

		private static SqlConnection DoGetDatabaseConnection(string databaseServer, string databaseName, string databaseUserName, string databasePassword)
		{
			var csb = new SqlConnectionStringBuilder();
			csb.DataSource = databaseServer;
			csb.InitialCatalog = databaseName;
			csb.UserID = databaseUserName;
			csb.Password = databasePassword;
			return new SqlConnection(csb.ToString());
		}

		public static bool DatabaseExists(SqlConnection conn, string databaseName)
		{
			if (conn.State != ConnectionState.Open)
				throw new InvalidOperationException("The database connection must be open to query for an existing database");
			var cmd = new SqlCommand(String.Format(DatabaseExistsQuery, databaseName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}

		public static bool LoginExists(SqlConnection conn, string loginName)
		{
			if (conn.State != ConnectionState.Open)
				throw new InvalidOperationException("The database connection must be open to query for an existing login");
			var cmd = new SqlCommand(String.Format(ExistingLoginQuery, loginName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}
	}
}