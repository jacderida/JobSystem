using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120315204615)]
	public class M20120315204615_AddPendingItemsTable : Migration
	{
		public override void Up()
		{
			Create.Table("PendingDeliveryItems")
				.WithIdColumn()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("QuoteItemId").AsGuid().Nullable()
				.WithColumn("CustomerId").AsGuid().NotNullable()
				.WithColumn("BeyondEconomicRepair").AsBoolean().NotNullable();
			Create.ForeignKey("").FromTable("PendingDeliveryItems").ForeignColumn("JobItemId").ToTable("JobItems").PrimaryColumn("Id");
		}

		public override void Down()
		{
			throw new System.NotImplementedException();
		}
	}
}