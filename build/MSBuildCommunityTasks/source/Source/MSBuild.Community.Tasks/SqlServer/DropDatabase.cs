using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks.SqlServer
{
	public class DropDatabase : Task
	{
		[Required] public string DatabaseServer { get; set; }
		[Required] public string DatabaseUserName { get; set; }
		[Required] public string DatabasePassword { get; set; }
		[Required] public string DatabaseName { get; set; }

		private const string DatabaseExistsQuery = "SELECT COUNT(*) FROM sysdatabases WHERE name='{0}'";

		public override bool Execute()
		{
			var conn = GetDatabaseConnection();
			try
			{
				conn.Open();
				if (!DatabaseExists(conn))
				{
					Log.LogMessage("Database {0} doesn't exist on {1}", DatabaseName, DatabaseServer);
					return true;
				}
				
				Log.LogMessage("Attempting to close connections to {0} on {1}", DatabaseName, DatabaseServer);
				var cmd = new SqlCommand(String.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", DatabaseName), conn);
				cmd.ExecuteNonQuery();

				Log.LogMessage("Dropping database {0} from {1}", DatabaseName, DatabaseServer);
				cmd.CommandText = String.Format("DROP DATABASE {0}", DatabaseName);
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

		private bool DatabaseExists(SqlConnection conn)
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			var cmd = new SqlCommand(String.Format(DatabaseExistsQuery, DatabaseName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}

		private SqlConnection GetDatabaseConnection()
		{
			var csb = new SqlConnectionStringBuilder();
			csb.DataSource = DatabaseServer;
			csb.UserID = DatabaseUserName;
			csb.Password = DatabasePassword;
			return new SqlConnection(csb.ToString());
		}
	}
}