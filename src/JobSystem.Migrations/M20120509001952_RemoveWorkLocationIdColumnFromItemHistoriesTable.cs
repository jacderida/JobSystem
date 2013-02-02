using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120509001952)]
    public class M20120509001952_RemoveWorkLocationIdColumnFromItemHistoriesTable : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_ItemHistory_WorkLocation").OnTable("ItemHistories");
            Delete.Column("WorkLocationId").FromTable("ItemHistories");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}