using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20120308205415)]
	public class M20120308205415_AddOrdersTable : Migration
	{
		public override void Up()
		{
			Create.Table("Orders")
				.WithIdColumn()
				.WithColumn("SupplierId").AsGuid().NotNullable()
				.WithColumn("OrderNo").AsString().NotNullable().Unique()
				.WithColumn("DateCreated").AsDateTime().NotNullable()
				.WithColumn("CreatedById").AsGuid().NotNullable()
				.WithColumn("Instructions").AsString(255).Nullable()
				.WithColumn("CurrencyId").AsGuid().NotNullable()
				.WithColumn("IsApproved").AsBoolean().NotNullable();
			Create.ForeignKey("FK_Orders_Suppliers").FromTable("Orders").ForeignColumn("SupplierId").ToTable("Suppliers").PrimaryColumn("Id");
			Create.ForeignKey("FK_Orders_UserAccounts").FromTable("Orders").ForeignColumn("CreatedById").ToTable("UserAccounts").PrimaryColumn("Id");
			Create.ForeignKey("FK_Orders_CurrencyId").FromTable("Orders").ForeignColumn("CurrencyId").ToTable("ListItems").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_Orders_Suppliers").OnTable("Orders");
			Delete.ForeignKey("FK_Orders_UserAccounts").OnTable("Orders");
			Delete.ForeignKey("FK_Orders_CurrencyId").OnTable("Orders");
			Delete.Table("Orders");
		}
	}
}