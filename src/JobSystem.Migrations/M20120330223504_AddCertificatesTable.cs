using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120330223504)]
	public class M20120330223504_AddCertificatesTable : Migration
	{
		public override void Up()
		{
			Create.Table("Certificates")
				.WithIdColumn()
				.WithColumn("CertificateNumber").AsString(255).Unique().NotNullable()
				.WithColumn("CreatedById").AsGuid().NotNullable()
				.WithColumn("DateCreated").AsDateTime().NotNullable()
				.WithColumn("TypeId").AsGuid().NotNullable()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("ProcedureList").AsString(255).Nullable();
			Create.ForeignKey("FK_Certificates_JobItems").FromTable("Certificates").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
			Create.ForeignKey("FK_Certificates_ListItems").FromTable("Certificates").ForeignColumn("TypeId").ToTable("ListItems").PrimaryColumn("Id");
			Create.Table("CertificatesTestStandards")
				.WithColumn("CertificateId").AsGuid().NotNullable()
				.WithColumn("TestStandardId").AsGuid().NotNullable();
			Create.ForeignKey("FK_CertificatesTestStandards_TestStandards")
				.FromTable("CertificatesTestStandards").ForeignColumn("TestStandardId").ToTable("TestStandards").PrimaryColumn("Id");
			Create.ForeignKey("FK_CertificatesTestStandards_Certificates")
				.FromTable("CertificatesTestStandards").ForeignColumn("CertificateId").ToTable("Certificates").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_CertificatesTestStandards_TestStandards").OnTable("CertificatesTestStandards");
			Delete.ForeignKey("FK_CertificatesTestStandards_Certificates").OnTable("CertificatesTestStandards");
			Delete.Table("CertificatesTestStandards");
			Delete.ForeignKey("FK_Certificates_JobItems").OnTable("Certificates");
			Delete.ForeignKey("FK_Certificates_ListItems").OnTable("Certificates");
			Delete.Table("Certificates");
		}
	}
}