using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120829204945)]
    public class M20120829204945_AddDeliveryLinesToCustomerTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Customers")
                .AddColumn("DeliveryAddress6").AsString(50).Nullable()
                .AddColumn("DeliveryAddress7").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("DeliveryAddress6").FromTable("Customers");
            Delete.Column("DeliveryAddress7").FromTable("Customers");
        }
    }
}