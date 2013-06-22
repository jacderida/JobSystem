using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130622015035)]
    public class M20130622015035_AddCategoryIdColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("Certificates").AddColumn("CategoryId").AsGuid().Nullable();
            Create.ForeignKey("FK_Certificates_Categories")
                .FromTable("Certificates")
                .ForeignColumn("CategoryId")
                .ToTable("ListItems")
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Certificates_Categories").OnTable("Certificates");
            Delete.Column("CategoryId").FromTable("Certificates");
        }
    }
}