﻿using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120304130625)]
    public class M20120304130625_AddPendingQuoteItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("PendingQuoteItems")
                .WithIdColumn()
                .WithColumn("OrderNo").AsString(50).Nullable()
                .WithColumn("AdviceNo").AsString(50).Nullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("JobItemId").AsGuid().NotNullable()
                .WithColumn("Labour").AsDecimal().NotNullable()
                .WithColumn("Calibration").AsDecimal().NotNullable()
                .WithColumn("Parts").AsDecimal().NotNullable()
                .WithColumn("Carriage").AsDecimal().NotNullable()
                .WithColumn("Investigation").AsDecimal().NotNullable()
                .WithColumn("Report").AsString(2000).Nullable()
                .WithColumn("Days").AsInt32().Nullable()
                .WithColumn("BeyondEconomicRepair").AsBoolean().NotNullable();
            Create.ForeignKey("FK_PendingQuoteItems_Customers").FromTable("PendingQuoteItems").ForeignColumn("CustomerId").ToTable("Customers").PrimaryColumn("Id");
            Create.ForeignKey("FK_PendingQuoteItems_JobItems").FromTable("PendingQuoteItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_PendingQuoteItems_Customers").OnTable("PendingQuoteItems");
            Delete.ForeignKey("FK_PendingQuoteItems_JobItems").OnTable("PendingQuoteItems");
            Delete.Table("PendingQuoteItems");
        }
    }
}