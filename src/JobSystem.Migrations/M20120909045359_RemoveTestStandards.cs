using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120909045359)]
    public class M20120909045359_RemoveTestStandards : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_CertificatesTestStandards_TestStandards").OnTable("CertificatesTestStandards");
            Delete.ForeignKey("FK_CertificatesTestStandards_Certificates").OnTable("CertificatesTestStandards");
            Delete.Table("CertificatesTestStandards");
            Delete.Table("TestStandards");
        }

        public override void Down()
        {
            Create.Table("TestStandards")
                .WithIdColumn()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("SerialNo").AsString(50).NotNullable()
                .WithColumn("CertificateNo").AsString(50).NotNullable();
            Create.Table("CertificatesTestStandards")
                .WithColumn("CertificateId").AsGuid().NotNullable()
                .WithColumn("TestStandardId").AsGuid().NotNullable();
            Create.ForeignKey("FK_CertificatesTestStandards_TestStandards")
                .FromTable("CertificatesTestStandards").ForeignColumn("TestStandardId").ToTable("TestStandards").PrimaryColumn("Id");
            Create.ForeignKey("FK_CertificatesTestStandards_Certificates")
                .FromTable("CertificatesTestStandards").ForeignColumn("CertificateId").ToTable("Certificates").PrimaryColumn("Id");
        }
    }
}