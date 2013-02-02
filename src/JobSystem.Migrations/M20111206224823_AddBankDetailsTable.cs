using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20111206224823)]
    public class M20111206224823_AddBankDetailsTable : Migration
    {
        public override void Up()
        {
            Create.Table("BankDetails")
                .WithIdColumn()
                .WithColumn("ShortName").AsString(50).NotNullable()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("AccountNo").AsString(50).NotNullable()
                .WithColumn("SortCode").AsString(50).Nullable()
                .WithColumn("Address1").AsString(50).Nullable()
                .WithColumn("Address2").AsString(50).Nullable()
                .WithColumn("Address3").AsString(50).Nullable()
                .WithColumn("Address4").AsString(50).Nullable()
                .WithColumn("Address5").AsString(50).Nullable()
                .WithColumn("Iban").AsString(50).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("BankDetails");
        }
    }
}