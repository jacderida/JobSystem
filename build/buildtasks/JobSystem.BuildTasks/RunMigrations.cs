using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JobSystem.BuildTasks
{
	public class RunMigrations : Task
	{
		[Required] public string MigratePath { get; set; }
		[Required] public string MigrationsAssemblyPath { get; set; }
		[Required] public string TenantListDatabaseServer { get; set; }
		[Required] public string TenantListDatabaseUserName { get; set; }
		[Required] public string TenantListDatabasePassword { get; set; }
		public string TenantListDatabaseName { get; set; }
		private List<string> _connectionStrings = new List<string>();

		public override bool Execute()
		{
			if (String.IsNullOrEmpty(TenantListDatabaseName))
				TenantListDatabaseName = "jobsystem.tenantlist";
			try
			{
				GetTenantConnectionStrings();
				DoRunMigrations();
				return true;
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex, true);
				return false;
			}
		}

		private void GetTenantConnectionStrings()
		{
			var conn = SqlServerHelper.GetDatabaseConnection(TenantListDatabaseServer, TenantListDatabaseName, TenantListDatabaseUserName, TenantListDatabasePassword);
			try
			{
				conn.Open();
				var cmd = GetTenantConnectionStringsCommand(conn);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						_connectionStrings.Add(reader.GetString(0));
				}
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				conn.Close();
			}
		}

		private SqlCommand GetTenantConnectionStringsCommand(SqlConnection conn)
		{
			var cmd = new SqlCommand("SELECT ConnectionString FROM TenantList", conn);
			return cmd;
		}

		private void DoRunMigrations()
		{
			foreach (var connectionString in _connectionStrings)
			{
				Log.LogMessage("Running migrations on {0}", connectionString);
				var process = new Process();
				process.StartInfo.FileName = MigratePath;
				process.StartInfo.Arguments = GetMigrateArguments(connectionString);
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.Start();
				var output = process.StandardOutput.ReadToEnd();
				var error = process.StandardError.ReadToEnd();
				process.WaitForExit();
				Log.LogMessage(output);
				if (process.ExitCode == -1 || !String.IsNullOrEmpty(error))
				{
					process.Close();
					throw new InvalidOperationException(String.Format("Migrations failed to run for {0}: {1}", connectionString, error));
				}
				process.Close();
			}
		}

		private string GetMigrateArguments(string connectionString)
		{
			var args = String.Format("-a {0} -db SqlServer2008 --conn \"{1}\"", MigrationsAssemblyPath, connectionString);
			Log.LogMessage(args);
			return args;
		}
	}
}