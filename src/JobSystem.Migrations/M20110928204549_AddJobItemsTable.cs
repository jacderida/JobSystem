using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928204549)]
	public class M20110928204549_AddJobItemsTable : Migration
	{
		public override void Up()
		{
			Create.Table("JobItems")
				.WithIdColumn()
				.WithColumn("JobId").AsGuid().NotNullable()
				.WithColumn("ItemNo").AsInt32().NotNullable()
				.WithColumn("Created").AsDateTime().NotNullable()
				.WithColumn("InstrumentId").AsGuid().NotNullable()
				.WithColumn("SerialNo").AsString(255).NotNullable()
				.WithColumn("AssetNo").AsString(255).Nullable()
				.WithColumn("UserAccountId").AsGuid().NotNullable()
				.WithColumn("InitialStatusId").AsGuid().NotNullable()
				.WithColumn("StatusId").AsGuid().NotNullable()
				.WithColumn("JobItemLocationId").AsGuid().NotNullable()
				.WithColumn("JobItemWorkLocationId").AsGuid().NotNullable()
				.WithColumn("JobItemOperatingUnitId").AsGuid().NotNullable()
				.WithColumn("CalPeriod").AsInt32().NotNullable()
				.WithColumn("Instructions").AsString(255).Nullable()
				.WithColumn("Accessories").AsString(20000).Nullable()
				.WithColumn("IsReturned").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("ReturnReason").AsString(255).NotNullable()
				.WithColumn("IsCertProduced").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("IsMarkedForInvoicing").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("Comments").AsString(255).Nullable()
				.WithColumn("IsInvoiced").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("ProjectedDeliveryDate").AsDateTime().Nullable()
				.WithColumn("IsMarkedForMonthlyInvoicing").AsBoolean().NotNullable().WithDefaultValue(false);
			Create.ForeignKey("FK_JobItems_Jobs")
				.FromTable("JobItems")
				.ForeignColumn("JobId")
				.ToTable("Jobs")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_InitialStatus")
				.FromTable("JobItems")
				.ForeignColumn("InitialStatusId")
				.ToTable("Status")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_Status")
				.FromTable("JobItems")
				.ForeignColumn("StatusId")
				.ToTable("Status")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_Instruments")
				.FromTable("JobItems")
				.ForeignColumn("InstrumentId")
				.ToTable("Instruments")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_ItemLocation")
				.FromTable("JobItems")
				.ForeignColumn("JobItemLocationId")
				.ToTable("JobItemLocations")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_WorkLocation")
				.FromTable("JobItems")
				.ForeignColumn("JobItemWorkLocationId")
				.ToTable("JobItemWorkLocations")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_OperatingUnit")
				.FromTable("JobItems")
				.ForeignColumn("JobItemOperatingUnitId")
				.ToTable("JobItemOperatingUnits")
				.PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_JobItems_OperatingUnit").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_WorkLocation").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_ItemLocation").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Instruments").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Status").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_InitialStatus").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Jobs").OnTable("JobItems");
			Delete.Table("JobItems");
		}
	}
}