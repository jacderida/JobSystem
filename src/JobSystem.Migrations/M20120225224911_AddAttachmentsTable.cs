using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120225224911)]
    public class M20120225224911_AddAttachmentsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Attachments")
                .WithIdColumn()
                .WithColumn("Filename").AsString(2000).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Attachments");
        }
    }
}