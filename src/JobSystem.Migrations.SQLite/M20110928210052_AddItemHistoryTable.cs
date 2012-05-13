using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20110928210052)]
	public class M20110928210052_AddItemHistoryTable : Migration
	{
		public override void Up()
		{
			Create.Table("ItemHistories")
				.WithIdColumn()
				.WithColumn("JobItemId").AsGuid().NotNullable()
				.WithColumn("DateCreated").AsDateTime().NotNullable()
				.WithColumn("WorkTypeId").AsGuid().NotNullable()
				.WithColumn("WorkTime").AsInt32().NotNullable().WithDefaultValue(0)
				.WithColumn("OverTime").AsInt32().NotNullable().WithDefaultValue(0)
				.WithColumn("Report").AsString(255).Nullable()
				.WithColumn("StatusId").AsGuid().NotNullable()
				.WithColumn("UserId").AsGuid().NotNullable();
			Create.ForeignKey("FK_ItemHistory_Status")
				.FromTable("ItemHistories")
				.ForeignColumn("StatusId")
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_JobItems")
				.FromTable("ItemHistories")
				.ForeignColumn("JobItemId")
				.ToTable("JobItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_WorkType")
				.FromTable("ItemHistories")
				.ForeignColumn("WorkTypeId")
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_ItemHistory_UserAccounts")
				.FromTable("ItemHistories")
				.ForeignColumn("UserId")
				.ToTable("UserAccounts")
				.PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_ItemHistory_UserAccounts").OnTable("ItemHistories");
			Delete.ForeignKey("FK_ItemHistory_WorkType").OnTable("ItemHistories");
			Delete.ForeignKey("FK_ItemHistory_WorkLocation").OnTable("ItemHistories");
			Delete.ForeignKey("FK_ItemHistory_JobItems").OnTable("ItemHistories");
			Delete.ForeignKey("FK_ItemHistory_Status").OnTable("ItemHistories");
			Delete.Table("ItemHistories");
		}
	}
}