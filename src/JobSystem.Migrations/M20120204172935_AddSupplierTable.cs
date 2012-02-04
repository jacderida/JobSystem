using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120204172935)]
	public class M20120204172935_AddSupplierTable : Migration
	{
		public override void Up()
		{
			Create.Table("Suppliers")
				.WithIdColumn()
				.WithColumn("Name").AsString(255).NotNullable()
				.WithColumn("Address1").AsString(50).Nullable()
				.WithColumn("Address2").AsString(50).Nullable()
				.WithColumn("Address3").AsString(50).Nullable()
				.WithColumn("Address4").AsString(50).Nullable()
				.WithColumn("Address5").AsString(50).Nullable()
				.WithColumn("Telephone").AsString(50).Nullable()
				.WithColumn("Fax").AsString(50).Nullable()
				.WithColumn("Email").AsString(50).Nullable()
				.WithColumn("SalesAddress1").AsString(50).Nullable()
				.WithColumn("SalesAddress2").AsString(50).Nullable()
				.WithColumn("SalesAddress3").AsString(50).Nullable()
				.WithColumn("SalesAddress4").AsString(50).Nullable()
				.WithColumn("SalesAddress5").AsString(50).Nullable()
				.WithColumn("Contact1").AsString(50).Nullable()
				.WithColumn("Contact2").AsString(50).Nullable()
				.WithColumn("SalesTelephone").AsString(50).Nullable()
				.WithColumn("SalesFax").AsString(50).Nullable()
				.WithColumn("SalesEmail").AsString(50).Nullable()
				.WithColumn("SalesContact1").AsString(50).Nullable()
				.WithColumn("SalesContact2").AsString(50).Nullable();
		}

		public override void Down()
		{
			Delete.Table("Suppliers");
		}
	}
}