using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20110928201038)]
    public class M20110928201038_AddListItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("ListItems")
                .WithIdColumn()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();
            Create.ForeignKey("FK_ListItems_ListItemCategories")
                .FromTable("ListItems")
                .ForeignColumn("CategoryId")
                .ToTable("ListItemCategories")
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_ListItems_ListItemCategories").OnTable("ListItems");
            Delete.Table("ListItems");
        }
    }
}