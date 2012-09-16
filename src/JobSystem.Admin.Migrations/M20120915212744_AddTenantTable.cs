using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Admin.Migrations
{
	[Migration(20120915212744)]
	public class M20120915212744_AddTenantTable : Migration
	{
		public override void Up()
		{
			Create.Table("Tenants")
				.WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
				.WithColumn("TenantName").AsString(50).NotNullable()
				.WithColumn("CompanyName").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("Tenants");
		}
	}
}