using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20111228143737)]
	public class M20111228143737_AddIsPendingColumnToJobsTable : Migration
	{
		public override void Up()
		{
			Create.Column("IsPending").OnTable("Jobs").AsBoolean().Nullable();
		}

		public override void Down()
		{
			Delete.Column("IsPending").FromTable("Jobs");
		}
	}
}