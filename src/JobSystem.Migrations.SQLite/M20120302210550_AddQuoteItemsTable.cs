using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20120302210550)]
	public class M20120302210550_AddQuoteItemsTable : Migration
	{
		public override void Up()
		{
			Create.Table("QuoteItems")
				.WithIdColumn()
				.WithColumn("QuoteId").AsGuid().NotNullable()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("DateAccepted").AsGuid().Nullable()
				.WithColumn("DateRejected").AsGuid().Nullable()
				.WithColumn("Labour").AsDecimal().Nullable()
				.WithColumn("Calibration").AsDecimal().Nullable()
				.WithColumn("Parts").AsDecimal().Nullable()
				.WithColumn("Investigation").AsDecimal().Nullable()
				.WithColumn("Report").AsString(2000).Nullable()
				.WithColumn("StatusId").AsGuid().NotNullable()
				.WithColumn("Days").AsInt32().Nullable()
				.WithColumn("BeyondEconomicRepair").AsBoolean().NotNullable();
			Create.ForeignKey("FK_QuoteItems_Quotes").FromTable("QuoteItems").ForeignColumn("QuoteId").ToTable("Quotes").PrimaryColumn("Id");
			Create.ForeignKey("FK_QuoteItems_JobItems").FromTable("QuoteItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
			Create.ForeignKey("FK_QuoteItems_StatusId").FromTable("QuoteItems").ForeignColumn("StatusId").ToTable("ListItems").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_QuoteItems_Quotes").OnTable("QuoteItems");
			Delete.ForeignKey("FK_QuoteItems_JobItems").OnTable("QuoteItems");
			Delete.ForeignKey("FK_QuoteItems_StatusId").OnTable("QuoteItems");
			Delete.Table("QuoteItems");
		}
	}
}