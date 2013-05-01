using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130501014535)]
    public class M20130501014535_AlterPendingQuoteItemReportColumn : Migration
    {
        public override void Up()
        {
            Alter.Column("Report").OnTable("PendingQuoteItems").AsString(int.MaxValue).Nullable();
        }

        public override void Down()
        {
            Alter.Column("Report").OnTable("PendingQuoteItems").AsString(2000).Nullable();
        }
    }
}