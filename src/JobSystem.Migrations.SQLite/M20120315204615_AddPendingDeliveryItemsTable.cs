using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20120315204615)]
	public class M20120315204615_AddPendingDeliveryItemsTable : Migration
	{
		public override void Up()
		{
			Create.Table("PendingDeliveryItems")
				.WithIdColumn()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("QuoteItemId").AsGuid().Nullable()
				.WithColumn("CustomerId").AsGuid().NotNullable()
				.WithColumn("Notes").AsString(255).Nullable()
				.WithColumn("BeyondEconomicRepair").AsBoolean().NotNullable();
			Create.ForeignKey("FK_PendingDeliveryItems_JobItems").FromTable("PendingDeliveryItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
			Create.ForeignKey("FK_PendingDeliveryItems_Customers").FromTable("PendingDeliveryItems").ForeignColumn("CustomerId").ToTable("Customers").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_PendingDeliveryItems_JobItems").OnTable("PendingDeliveryItems");
			Delete.ForeignKey("FK_PendingDeliveryItems_Customers").OnTable("PendingDeliveryItems");
			Delete.Table("DeliveryItems");
		}
	}
}