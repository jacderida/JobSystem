using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120520162923)]
    public class M20120520162923_ChangeCurrencyForeignKeyOnInvoicesTable : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_Invoices_Currency").OnTable("Invoices");
            Create.ForeignKey("FK_Invoices_Currencies").FromTable("Invoices").ForeignColumn("CurrencyId").ToTable("Currencies").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Invoices_Currencies").OnTable("Invoices");
            Create.ForeignKey("FK_Invoices_Currency").FromTable("Invoices").ForeignColumn("CurrencyId").ToTable("ListItems").PrimaryColumn("Id");
        }
    }
}