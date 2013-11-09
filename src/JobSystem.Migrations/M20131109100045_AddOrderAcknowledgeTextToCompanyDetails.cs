using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20131109100045)]
	public class M20131109100045_AddOrderAcknowledgeTextToCompanyDetails : Migration
	{
		public override void Up()
		{
			Alter.Table("CompanyDetails").AddColumn("OrderAcknowledgeText").AsString(int.MaxValue).Nullable().WithDefaultValue(
				"Can you acknowledge this order and advise of a delivery date as soon as possible? The delivery address is as stated below, unless you are advised otherwise.");
		}

		public override void Down()
		{
			Delete.Column("OrderAcknowledgeText").FromTable("CompanyDetails");
		}
	}
}
