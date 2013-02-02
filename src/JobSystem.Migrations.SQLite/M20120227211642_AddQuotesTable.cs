using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20120227211642)]
    public class M20120227211642_AddQuotesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Quotes")
                .WithIdColumn()
                .WithColumn("QuoteNumber").AsString(50).Unique().NotNullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("CreatedById").AsGuid().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable()
                .WithColumn("OrderNumber").AsString(50).Nullable()
                .WithColumn("AdviceNumber").AsString(50).Nullable()
                .WithColumn("CurrencyId").AsGuid().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable()
                .WithColumn("Revision").AsInt32().NotNullable();
            Create.ForeignKey("FK_Quotes_Customers").FromTable("Quotes").ForeignColumn("CustomerId").ToTable("Customers").PrimaryColumn("Id");
            Create.ForeignKey("FK_Quotes_UserAccounts").FromTable("Quotes").ForeignColumn("CreatedById").ToTable("UserAccounts").PrimaryColumn("Id");
            Create.ForeignKey("FK_Quotes_CurrencyId").FromTable("Quotes").ForeignColumn("CurrencyId").ToTable("ListItems").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Quotes_Customers").OnTable("Quotes");
            Delete.ForeignKey("FK_Quotes_UserAccounts").OnTable("Quotes");
            Delete.ForeignKey("FK_Quotes_CurrencyId").OnTable("Quotes");
            Delete.Table("Quotes");
        }
    }
}