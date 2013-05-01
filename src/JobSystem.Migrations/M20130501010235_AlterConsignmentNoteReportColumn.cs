using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130501010235)]
    public class M20130501010235_AlterConsignmentNoteReportColumn : Migration
    {
        public override void Up()
        {
            Alter.Column("Instructions").OnTable("ConsignmentItems").AsString(int.MaxValue);
        }

        public override void Down()
        {
            Alter.Column("Instructions").OnTable("ConsignmentItems").AsString(255);
        }
    }
}