using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20120330222049)]
	public class M20120330222049_AddTestStandardsTable : Migration
	{
		public override void Up()
		{
			Create.Table("TestStandards")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable()
				.WithColumn("SerialNo").AsString(50).NotNullable()
				.WithColumn("CertificateNo").AsString(50).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("TestStandards");
		}
	}
}