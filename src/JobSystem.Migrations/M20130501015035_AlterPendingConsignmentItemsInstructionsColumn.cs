using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130501015035)]
    public class M20130501015035_AlterPendingConsignmentItemsInstructionsColumn : Migration
    {
        public override void Up()
        {
            Alter.Column("Instructions").OnTable("PendingConsignmentItems").AsString(int.MaxValue);
        }

        public override void Down()
        {
            Alter.Column("Instructions").OnTable("PendingConsignmentItems").AsString(2000);
        }
    }
}