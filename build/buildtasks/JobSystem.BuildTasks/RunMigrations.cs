﻿using System;
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
				Log.LogMessage("Args: {0}", process.StartInfo.Arguments);
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();
				var output = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
				Log.LogMessage(output);
				if (process.ExitCode == -1)
					throw new InvalidOperationException(String.Format("Migrations failed to run for {0}", connectionString));
			}
		}

		private string GetMigrateArguments(string connectionString)
		{
			return String.Format("-a {0} -db SqlServer2008 --conn \"{1}\"", MigrationsAssemblyPath, connectionString);
		}
	}
}