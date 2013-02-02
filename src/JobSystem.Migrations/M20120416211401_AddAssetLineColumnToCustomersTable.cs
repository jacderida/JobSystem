using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120416211401)]
    public class M20120416211401_AddAssetLineColumnToCustomersTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Customers").AddColumn("AssetLine").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("AssetLine").FromTable("Customers");
        }
    }
}