using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130501011035)]
    public class M20130501011035_AlterQuoteItemReportColumn : Migration
    {
        public override void Up()
        {
            Alter.Column("Report").OnTable("QuoteItems").AsString(int.MaxValue);
        }

        public override void Down()
        {
            Alter.Column("Report").OnTable("QuoteItems").AsString(2000);
        }
    }
}