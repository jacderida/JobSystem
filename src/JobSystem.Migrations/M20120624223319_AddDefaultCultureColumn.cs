using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120624223319)]
	public class M20120624223319_AddDefaultCultureColumn : Migration
	{
		public override void Up()
		{
			Alter.Table("CompanyDetails").AddColumn("DefaultCultureCode").AsString(50).Nullable();
		}

		public override void Down()
		{
			Delete.Column("DefaultCultureCode").FromTable("CompanyDetails");
		}
	}
}