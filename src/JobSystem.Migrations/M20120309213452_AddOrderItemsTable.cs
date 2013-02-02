using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120309213452)]
    public class M20120309213452_AddOrderItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("OrderItems")
                .WithIdColumn()
                .WithColumn("OrderId").AsGuid().NotNullable()
                .WithColumn("ItemNo").AsInt32().NotNullable()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("Quantity").AsInt32().NotNullable()
                .WithColumn("PartNo").AsString(50).Nullable()
                .WithColumn("Instructions").AsString(2000).Nullable()
                .WithColumn("DeliveryDays").AsInt32().Nullable()
                .WithColumn("DateReceived").AsDateTime().Nullable()
                .WithColumn("JobItemId").AsGuid().Nullable()
                .WithColumn("Price").AsDecimal().NotNullable()
                .WithColumn("InvoiceReceived").AsBoolean().NotNullable();
            Create.ForeignKey("FK_OrderItems_Orders").FromTable("OrderItems").ForeignColumn("OrderId").ToTable("Orders").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OrderItems_Orders").OnTable("OrderItems");
            Delete.Table("OrderItems");
        }
    }
}