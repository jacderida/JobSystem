using System;
using System.IO;
using System.Reflection;
using FluentMigrator.Runner.Initialization;

namespace JobSystem.Framework
{
    public static class MigrationsSchemaBuilder
    {
        public static void CreateSchemaFromMigrations(string migrationsAssemblyPath, string connectionString, string dbType)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var executingAssemblyPath = new Uri(executingAssembly.CodeBase).LocalPath;
            var binPath = Path.GetDirectoryName(executingAssemblyPath);
            var setup = new AppDomainSetup()
            {
                ApplicationBase = binPath,
                PrivateBinPath = binPath
            };
            AppDomain otherDomain = AppDomain.CreateDomain("migrations_runner_app_domain", null, setup);
            var runner = (CrossDomainMigrationRunner)otherDomain.CreateInstanceAndUnwrap(
                executingAssembly.FullName,
                typeof(CrossDomainMigrationRunner).FullName);
            runner.Run(migrationsAssemblyPath, connectionString, dbType);
            runner = null;
            AppDomain.Unload(otherDomain);
        }
    }

    public class CrossDomainMigrationRunner : MarshalByRefObject
    {
        public void Run(string migrationsDllPath, string connectionString, string dbType)
        {
            var announcer = new FluentMigrator.Runner.Announcers.NullAnnouncer();
            var migrationContext = new RunnerContext(announcer)
            {
                Database = dbType,
                Connection = connectionString,
                Target = migrationsDllPath,
                PreviewOnly = false
            };
            var taskExecuter = new TaskExecutor(migrationContext);
            taskExecuter.Execute();
        }
    }
}