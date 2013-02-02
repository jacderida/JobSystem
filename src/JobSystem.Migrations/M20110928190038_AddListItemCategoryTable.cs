using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20110928190038)]
    public class M20110928190038_AddListItemCategoryTable : Migration
    {
        public override void Up()
        {
            Create.Table("ListItemCategories")
                .WithIdColumn()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable();
            Execute.Sql(
                @"ALTER TABLE dbo.ListItemCategories ADD CONSTRAINT
                    IX_ListItemCategories UNIQUE NONCLUSTERED 
                    (
                    Name,
                    Type
                    )"
                );
        }

        public override void Down()
        {
            Delete.Table("ListItemCategories");
        }
    }
}