using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20110928201039)]
    public class M20110928201039_AddCustomerTable : Migration
    {
        public override void Up()
        {
            Create.Table("Customers")
                .WithIdColumn()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Address1").AsString(50).Nullable()
                .WithColumn("Address2").AsString(50).Nullable()
                .WithColumn("Address3").AsString(50).Nullable()
                .WithColumn("Address4").AsString(50).Nullable()
                .WithColumn("Address5").AsString(50).Nullable()
                .WithColumn("Telephone").AsString(50).Nullable()
                .WithColumn("Fax").AsString(50).Nullable()
                .WithColumn("Email").AsString(50).Nullable()
                .WithColumn("InvoiceTitle").AsString(255).Nullable()
                .WithColumn("InvoiceAddress1").AsString(50).Nullable()
                .WithColumn("InvoiceAddress2").AsString(50).Nullable()
                .WithColumn("InvoiceAddress3").AsString(50).Nullable()
                .WithColumn("InvoiceAddress4").AsString(50).Nullable()
                .WithColumn("InvoiceAddress5").AsString(50).Nullable()
                .WithColumn("Contact1").AsString(50).Nullable()
                .WithColumn("Contact2").AsString(50).Nullable()
                .WithColumn("SalesTelephone").AsString(50).Nullable()
                .WithColumn("SalesFax").AsString(50).Nullable()
                .WithColumn("SalesEmail").AsString(50).Nullable()
                .WithColumn("SalesContact1").AsString(50).Nullable()
                .WithColumn("SalesContact2").AsString(50).Nullable()
                .WithColumn("DeliveryTitle").AsString(255).Nullable()
                .WithColumn("DeliveryAddress1").AsString(50).Nullable()
                .WithColumn("DeliveryAddress2").AsString(50).Nullable()
                .WithColumn("DeliveryAddress3").AsString(50).Nullable()
                .WithColumn("DeliveryAddress4").AsString(50).Nullable()
                .WithColumn("DeliveryAddress5").AsString(50).Nullable()
                .WithColumn("DeliveryTelephone").AsString(50).Nullable()
                .WithColumn("DeliveryFax").AsString(50).Nullable()
                .WithColumn("DeliveryEmail").AsString(50).Nullable()
                .WithColumn("DeliveryContact1").AsString(50).Nullable()
                .WithColumn("DeliveryContact2").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Customers");
        }
    }
}