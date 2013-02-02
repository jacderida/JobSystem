using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20120212155352)]
    public class M20120212155352_AddPendingConsignmentItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("PendingConsignmentItems")
                .WithIdColumn()
                .WithColumn("SupplierId").AsGuid().NotNullable()
                .WithColumn("JobItemId").AsGuid().NotNullable()
                .WithColumn("Instructions").AsString(255).Nullable();
            Create.ForeignKey("FK_PendingConsignmentItems_Suppliers")
                .FromTable("PendingConsignmentItems").ForeignColumn("SupplierId")
                .ToTable("Suppliers").PrimaryColumn("Id");
            Create.ForeignKey("FK_PendingConsignmentItems_JobItems")
                .FromTable("PendingConsignmentItems").ForeignColumn("JobItemId")
                .ToTable("JobItems").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_PendingConsignmentItems_Suppliers").OnTable("PendingConsignmentItems");
            Delete.ForeignKey("FK_PendingConsignmentItems_JobItems").OnTable("PendingConsignmentItems");
            Delete.Table("PendingConsignmentItems");
        }
    }
}