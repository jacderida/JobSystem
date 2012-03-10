using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120310173501)]
	public class M20120310173501_AddPendingOrderItemsTable : Migration
	{
		public override void Up()
		{
			Create.Table("PendingOrderItems")
				.WithIdColumn()
				.WithColumn("SupplierId").AsGuid().NotNullable()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("Quantity").AsInt32().NotNullable()
				.WithColumn("PartNo").AsString(50).Nullable()
				.WithColumn("Instructions").AsString(2000).Nullable()
				.WithColumn("DeliveryDays").AsInt32().Nullable()
				.WithColumn("Price").AsDecimal().NotNullable();
			Create.ForeignKey("FK_PendingOrderItems_Suppliers").FromTable("PendingOrderItems").ForeignColumn("SupplierId").ToTable("Suppliers").PrimaryColumn("Id");
			Create.ForeignKey("FK_PendingOrderItems_JobItems").FromTable("PendingOrderItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_PendingOrderItems_Suppliers").OnTable("PendingOrderItems");
			Delete.ForeignKey("FK_PendingOrderItems_JobItems").OnTable("PendingOrderItems");
			Delete.Table("PendingOrderItems");
		}
	}
}