using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks.SqlServer
{
	public class CreateDatabase : Task
	{
		[Required] public string DatabaseServer { get; set; }
		[Required] public string DatabaseUserName { get; set; }
		[Required] public string DatabasePassword { get; set; }
		[Required] public string DatabaseName { get; set; }
		
		private const string DropDatabase = "DROP DATABASE [{0}]";

		public override bool Execute()
		{
			var conn = SqlServerHelper.GetDatabaseConnection(DatabaseServer, DatabaseUserName, DatabasePassword);
			try
			{
				conn.Open();
				var cmd = new SqlCommand();
				cmd.Connection = conn;
				if (SqlServerHelper.DatabaseExists(conn, DatabaseName))
				{
					cmd.CommandText = String.Format(DropDatabase, DatabaseName);
					cmd.ExecuteNonQuery();
				}
				Log.LogMessage("Creating database {0} on {1}", DatabaseName, DatabaseServer);
				cmd.CommandText = String.Format("CREATE DATABASE [{0}]", DatabaseName);
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
	}
}