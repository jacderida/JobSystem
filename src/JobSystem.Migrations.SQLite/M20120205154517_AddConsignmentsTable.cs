using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20120205154517)]
	public class M20120205154517_AddConsignmentsTable : Migration
	{
		public override void Up()
		{
			Create.Table("Consignments")
				.WithIdColumn()
				.WithColumn("ConsignmentNo").AsString(255).Unique().NotNullable()
				.WithColumn("DateCreated").AsDateTime().NotNullable()
				.WithColumn("SupplierId").AsGuid().NotNullable()
				.WithColumn("CreatedById").AsGuid().NotNullable();
			Create.ForeignKey("FK_Consignments_UserAccounts")
				.FromTable("Consignments")
				.ForeignColumn("CreatedById")
				.ToTable("UserAccounts")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_Consignments_Suppliers")
				.FromTable("Consignments")
				.ForeignColumn("SupplierId")
				.ToTable("Suppliers")
				.PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_Consignments_UserAccounts").OnTable("Consignments");
			Delete.ForeignKey("FK_Consignments_Suppliers").OnTable("Consignments");
			Delete.Table("Consignments");
		}
	}
}