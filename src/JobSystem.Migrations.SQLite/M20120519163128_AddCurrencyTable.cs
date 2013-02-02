using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20120519163128)]
    public class M20120519163128_AddCurrencyTable : Migration
    {
        public override void Up()
        {
            Create.Table("Currencies")
                .WithIdColumn()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("DisplayMessage").AsString(50).NotNullable();

            Delete.ForeignKey("FK_CompanyDetails_Currencies").OnTable("CompanyDetails");
            Create.ForeignKey("FK_CompanyDetails_Currencies")
                .FromTable("CompanyDetails")
                .ForeignColumn("DefaultCurrencyId")
                .ToTable("Currencies")
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Currencies");
            Delete.ForeignKey("FK_CompanyDetails_Currencies").OnTable("CompanyDetails");
            Create.ForeignKey("FK_CompanyDetails_Currencies")
                .FromTable("CompanyDetails")
                .ForeignColumn("DefaultCurrencyId")
                .ToTable("ListItem")
                .PrimaryColumn("Id");
        }
    }
}