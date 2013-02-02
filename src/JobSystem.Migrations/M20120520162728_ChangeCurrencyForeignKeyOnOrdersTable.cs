using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120520162728)]
    public class M20120520162728_ChangeCurrencyForeignKeyOnOrdersTable : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_Orders_CurrencyId").OnTable("Orders");
            Create.ForeignKey("FK_Orders_Currencies").FromTable("Orders").ForeignColumn("CurrencyId").ToTable("Currencies").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Orders_Currencies").OnTable("Orders");
            Create.ForeignKey("FK_Orders_CurrencyId").FromTable("Orders").ForeignColumn("CurrencyId").ToTable("ListItems").PrimaryColumn("Id");
        }
    }
}