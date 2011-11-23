namespace MSBuild.Community.Tasks.SqlAzure
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

	public class DropDatabase : Task
	{
		[Required] public string DatabaseServer { get; set; }
		[Required] public string DatabaseUserName { get; set; }
		[Required] public string DatabasePassword { get; set; }
		[Required] public string DatabaseName { get; set; }

		private const string DatabaseExistsQuery = "SELECT COUNT(*) FROM sysdatabases WHERE name='{0}'";

		public override bool Execute()
		{
            // This is basically a copy of the standard SQL DropDatabase task, however SQL Azure doesn't support the
            // single_user db_user_access_option (http://msdn.microsoft.com/en-us/library/ff394109.aspx), so it's been
            // removed.

			var conn = GetDatabaseConnection();
			try
			{
				conn.Open();
				if (!DatabaseExists(conn))
				{
					Log.LogMessage("Database {0} doesn't exist on {1}", DatabaseName, DatabaseServer);
					return true;
				}

				Log.LogMessage("Dropping database {0} from {1}", DatabaseName, DatabaseServer);
				var cmd = new SqlCommand(String.Format("DROP DATABASE {0}", DatabaseName), conn);
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