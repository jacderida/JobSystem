using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120405212803)]
	public class M20120405212803_AddInvoicesItemsTable : Migration
	{
		public override void Up()
		{
			Create.Table("InvoiceItems")
				.WithIdColumn()
				.WithColumn("InvoiceId").AsGuid().NotNullable()
				.WithColumn("ItemNo").AsInt32().NotNullable()
				.WithColumn("Description").AsString(255).NotNullable()
				.WithColumn("Price").AsDecimal().NotNullable()
				.WithColumn("CalibrationPrice").AsDecimal().NotNullable()
				.WithColumn("RepairPrice").AsDecimal().NotNullable()
				.WithColumn("PartsPrice").AsDecimal().NotNullable()
				.WithColumn("CarriagePrice").AsDecimal().NotNullable()
				.WithColumn("InvestigationPrice").AsDecimal().NotNullable()
				.WithColumn("JobItemId").AsGuid().Nullable();
			Create.ForeignKey("FK_InvoiceItems_Invoices").FromTable("InvoiceItems").ForeignColumn("InvoiceId").ToTable("Invoices").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_InvoiceItems_Invoices").OnTable("InvoiceItems");
			Delete.Table("InvoiceItems");
		}
	}
}