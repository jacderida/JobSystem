using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928210052)]
	public class M20110928210052_AddItemHistoryTable : Migration
	{
		public override void Up()
		{
			Create.Table("ItemHistory")
				.WithIdColumn()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("DateCreated").AsDateTime().NotNullable()
				.WithColumn("JobItemWorkTypeId").AsGuid().NotNullable()
				.WithColumn("WorkTime").AsInt32().NotNullable().WithDefaultValue(0)
				.WithColumn("OverTime").AsInt32().NotNullable().WithDefaultValue(0)
				.WithColumn("Report").AsString(255).Nullable()
				.WithColumn("StatusId").AsGuid().NotNullable()
				.WithColumn("JobItemWorkLocationId").AsGuid().NotNullable()
				.WithColumn("UserAccountId").AsGuid().NotNullable();
			Create.ForeignKey("FK_ItemHistory_Status")
				.FromTable("ItemHistory")
				.ForeignColumn("StatusId")
				.ToTable("Status")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_JobItems")
				.FromTable("ItemHistory")
				.ForeignColumn("JobItemId")
				.ToTable("JobItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_WorkLocation")
				.FromTable("ItemHistory")
				.ForeignColumn("JobItemWorkLocationId")
				.ToTable("JobItemWorkLocations")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_WorkType")
				.FromTable("ItemHistory")
				.ForeignColumn("JobItemWorkTypeId")
				.ToTable("JobItemWorkTypes")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_UserAccounts")
				.FromTable("ItemHistory")
				.ForeignColumn("UserAccountId")
				.ToTable("UserAccounts")
				.PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_ItemHistory_UserAccounts").OnTable("ItemHistory");
			Delete.ForeignKey("FK_ItemHistory_WorkType").OnTable("ItemHistory");
			Delete.ForeignKey("FK_ItemHistory_WorkLocation").OnTable("ItemHistory");
			Delete.ForeignKey("FK_ItemHistory_JobItems").OnTable("ItemHistory");
			Delete.ForeignKey("FK_ItemHistory_Status").OnTable("ItemHistory");
			Delete.Table("ItemHistory");
		}
	}
}