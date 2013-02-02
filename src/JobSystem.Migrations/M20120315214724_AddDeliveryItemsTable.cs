using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120315214724)]
    public class M20120315214724_AddDeliveryItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("DeliveryItems")
                .WithIdColumn()
                .WithColumn("DeliveryId").AsGuid().NotNullable()
                .WithColumn("ItemNo").AsInt32().NotNullable()
                .WithColumn("JobItemId").AsGuid().NotNullable()
                .WithColumn("QuoteItemId").AsGuid().Nullable()
                .WithColumn("Notes").AsString(255).Nullable();
            Create.ForeignKey("FK_DeliveryItems_Deliveries").FromTable("DeliveryItems").ForeignColumn("DeliveryId").ToTable("Deliveries").PrimaryColumn("Id");
            Create.ForeignKey("FK_DeliveryItems_JobItems").FromTable("DeliveryItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_DeliveryItems_Deliveries").OnTable("DeliveryItems");
            Delete.ForeignKey("FK_DeliveryItems_JobItems").OnTable("DeliveryItems");
            Delete.Table("DeliveryItems");
        }
    }
}