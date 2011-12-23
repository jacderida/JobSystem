using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator.Runner.Initialization;

namespace JobSystem.DbWireup
{
	public class CrossDomainMigrationRunner : MarshalByRefObject
	{
		public void Run(string migrationsDllPath, string connectionString)
		{
			var announcer = new FluentMigrator.Runner.Announcers.NullAnnouncer();
			var migrationContext = new RunnerContext(announcer)
			{
				Database = "sqlserver",
				Connection = connectionString,
				Target = migrationsDllPath,
				PreviewOnly = false
			};
			var taskExecuter = new TaskExecutor(migrationContext);
			taskExecuter.Execute();
		}
	}
}