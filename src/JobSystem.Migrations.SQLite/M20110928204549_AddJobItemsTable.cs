using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
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
				.WithColumn("CreatedUserId").AsGuid().NotNullable()
				.WithColumn("InitialStatusId").AsGuid().NotNullable()
				.WithColumn("StatusId").AsGuid().NotNullable()
				.WithColumn("LocationId").AsGuid().NotNullable()
				.WithColumn("FieldId").AsGuid().NotNullable()
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
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_Status")
				.FromTable("JobItems")
				.ForeignColumn("StatusId")
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_Fields")
				.FromTable("JobItems")
				.ForeignColumn("FieldId")
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_Instruments")
				.FromTable("JobItems")
				.ForeignColumn("InstrumentId")
				.ToTable("Instruments")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_ItemLocation")
				.FromTable("JobItems")
				.ForeignColumn("LocationId")
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_JobItems_UserAccounts")
				.FromTable("JobItems")
				.ForeignColumn("CreatedUserId")
				.ToTable("UserAccounts")
				.PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_JobItems_UserAccounts").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_ItemLocation").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Instruments").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Fields").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Status").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_InitialStatus").OnTable("JobItems");
			Delete.ForeignKey("FK_JobItems_Jobs").OnTable("JobItems");
			Delete.Table("JobItems");
		}
	}
}