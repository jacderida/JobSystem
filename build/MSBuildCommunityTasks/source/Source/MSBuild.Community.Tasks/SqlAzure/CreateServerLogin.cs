using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks.SqlAzure
{
	public class CreateServerLogin : Task
	{
		[Required]
		public string DatabaseServer { get; set; }
		[Required]
		public string DatabaseUserName { get; set; }
		[Required]
		public string DatabasePassword { get; set; }
		[Required]
		public string LoginName { get; set; }
		[Required]
		public string LoginPassword { get; set; }

		private const string CreateLoginCommandText = "CREATE LOGIN [{0}] WITH PASSWORD=N'{1}'";
		private const string CreateUserCommandText = "CREATE USER {0}";
		private const string ExistingLoginQuery = "select COUNT(*) from [sys].[sql_logins] WHERE name = '{0}'";
		private const string ModifyLoginRoleCommandText = "EXEC master..sp_addrolemember @membername = N'{0}', @rolename = N'dbmanager'";

		public override bool Execute()
		{
			var conn = GetDatabaseConnection();
			try
			{
				conn.Open();
				if (LoginExists(conn))
				{
					Log.LogMessage("The login {0} already exists on {1}", LoginName, DatabaseServer);
					return true;
				}
				var cmd = new SqlCommand(String.Format(CreateLoginCommandText, LoginName, LoginPassword), conn);
				cmd.ExecuteNonQuery();
				cmd.CommandText = String.Format(CreateUserCommandText, LoginName);
				cmd.ExecuteNonQuery();
				cmd.CommandText = String.Format(ModifyLoginRoleCommandText, LoginName);
				cmd.ExecuteNonQuery();
				return true;
			}
			catch (SqlException ex)
			{
				Log.LogErrorFromException(ex, true);
				return false;
			}
			finally
			{
				conn.Close();
			}
		}

		private SqlConnection GetDatabaseConnection()
		{
			var csb = new SqlConnectionStringBuilder();
			csb.DataSource = DatabaseServer;
			csb.UserID = DatabaseUserName;
			csb.Password = DatabasePassword;
			return new SqlConnection(csb.ToString());
		}

		private bool LoginExists(SqlConnection conn)
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			var cmd = new SqlCommand(String.Format(ExistingLoginQuery, LoginName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}
	}
}