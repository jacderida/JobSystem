using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120315204614)]
    public class M20120315204615_AddPendingItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Deliveries")
                .WithIdColumn()
                .WithColumn("DeliveryNoteNumber").AsString(255).Unique().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("CreatedById").AsGuid().NotNullable()
                .WithColumn("Fao").AsString(255).Nullable();
            Create.ForeignKey("FK_Deliveries_Customers").FromTable("Deliveries").ForeignColumn("CustomerId").ToTable("Customers").PrimaryColumn("Id");
            Create.ForeignKey("FK_Deliveries_UserAccounts").FromTable("Deliveries").ForeignColumn("CreatedById").ToTable("UserAccounts").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Deliveries_Customers").OnTable("Deliveries");
            Delete.ForeignKey("FK_Deliveries_UserAccounts").OnTable("Deliveries");
            Delete.Table("DeliveryItems");
        }
    }
}