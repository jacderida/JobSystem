using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120407040730)]
    public class M20120407040730_AddPendingInvoiceItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("PendingInvoiceItems")
                .WithIdColumn()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("CalibrationPrice").AsDecimal().NotNullable()
                .WithColumn("RepairPrice").AsDecimal().NotNullable()
                .WithColumn("PartsPrice").AsDecimal().NotNullable()
                .WithColumn("CarriagePrice").AsDecimal().NotNullable()
                .WithColumn("InvestigationPrice").AsDecimal().NotNullable()
                .WithColumn("JobItemId").AsGuid().Nullable()
                .WithColumn("OrderNo").AsString(50).Nullable();
            Create.ForeignKey("FK_PendingInvoiceItems_JobItems").FromTable("PendingInvoiceItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_PendingInvoiceItems_JobItems").OnTable("PendingInvoiceItems");
            Delete.Table("PendingInvoiceItems");
        }
    }
}