using System;
using System.Data.SQLite;
using NHibernate.Connection;
using NUnit.Framework;
using JobSystem.Framework;
using JobSystem.DataAccess.NHibernate.Mappings;
using JobSystem.DataAccess.NHibernate;

namespace JobSystem.BusinessLogic.Tests
{
	public class SearchTest
	{
		[TestFixtureSetUp]
		public void InitNHibernateSqlLite()
		{
			SQLiteConnection.CreateFile("JobSystem.Development.db3");
			MigrationsSchemaBuilder.CreateSchemaFromMigrations("JobSystem.Migrations.SQLite.dll", "Data Source=JobSystem.Development.db3", "sqlite");
			var dbConfigurer = FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard.ConnectionString("Data Source=JobSystem.Development.db3").Provider<SimpleConnectionProvider>();
			NHibernateSession.Init(
				new SimpleSessionStorage(), new[] { "JobSystem.DataAccess.NHibernate.dll" }, new AutoPersistenceModelGenerator().Generate(), null, null, null, dbConfigurer);
		}
	}

	public class SimpleConnectionProvider : DriverConnectionProvider
	{
		public static string CatalogName { get; set; }

		protected override string ConnectionString
		{
			get
			{
				return "Data Source=JobSystem.Development.db3";
			}
		}
	}
}