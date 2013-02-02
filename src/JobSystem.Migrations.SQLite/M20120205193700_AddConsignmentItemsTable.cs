using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20120205193700)]
    public class M20120205193700_AddConsignmentItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("ConsignmentItems")
                .WithIdColumn()
                .WithColumn("ConsignmentId").AsGuid().NotNullable()
                .WithColumn("ItemNo").AsInt32().NotNullable()
                .WithColumn("JobItemId").AsGuid().NotNullable()
                .WithColumn("Instructions").AsString(255).Nullable();
            Create.ForeignKey("FK_ConsignmentItems_Consignments")
                .FromTable("ConsignmentItems")
                .ForeignColumn("ConsignmentId")
                .ToTable("Consignments")
                .PrimaryColumn("Id");
            Create.ForeignKey("FK_ConsignmentItems_JobItems")
                .FromTable("ConsignmentItems")
                .ForeignColumn("JobItemId")
                .ToTable("JobItems")
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_ConsignmentItems_Consignments").OnTable("ConsignmentItems");
            Delete.ForeignKey("FK_ConsignmentItems_JobItems").OnTable("ConsignmentItems");
            Delete.Table("ConsignmentItems");
        }
    }
}