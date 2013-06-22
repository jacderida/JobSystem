using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130622015035)]
    public class M20130622015035_AddCategoryIdColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("Certificates").AddColumn("CategoryId").AsGuid().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CategoryId").FromTable("Certificates");
        }
    }
}