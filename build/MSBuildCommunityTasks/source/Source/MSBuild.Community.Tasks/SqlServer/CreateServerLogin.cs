using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks.SqlServer
{
	public class CreateServerLogin : Task
	{
		[Required] public string DatabaseServer { get; set; }
		[Required] public string DatabaseUserName { get; set; }
		[Required] public string DatabasePassword { get; set; }
		[Required] public string LoginName { get; set; }
		[Required] public string LoginPassword { get; set; }

		private const string CreateLoginCommandText = "CREATE LOGIN [{0}] WITH PASSWORD=N'{1}', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF";		
		private const string ModifyLoginRoleCommandText = "EXEC master..sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'";

		public override bool Execute()
		{
			var conn = SqlServerHelper.GetDatabaseConnection(DatabaseServer, DatabaseUserName, DatabasePassword);
			try
			{
				conn.Open();
				if (SqlServerHelper.LoginExists(conn, LoginName))
				{
					Log.LogMessage("The login {0} already exists on {1}", LoginName, DatabaseServer);
					return true;
				}
				var cmd = new SqlCommand(String.Format(CreateLoginCommandText, LoginName, LoginPassword), conn);
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
	}
}