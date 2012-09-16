using FluentMigrator;

namespace JobSystem.Admin.Migrations
{
	[Migration(20120915220146)]
	public class M20120915220146_AddTenantConnectionStringsTable : Migration
	{
		public override void Up()
		{
			Create.Table("TenantConnectionStrings")
				.WithColumn("Name").AsString(50).NotNullable()
				.WithColumn("ConnectionString").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("TenantConnectionStrings");
		}
	}
}