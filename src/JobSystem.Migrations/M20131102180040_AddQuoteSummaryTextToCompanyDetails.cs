using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20131102180040)]
	public class M20131102180040_AddQuoteSummaryTextToCompanyDetails : Migration
	{
		public override void Up()
		{
			Alter.Table("CompanyDetails").AddColumn("QuoteSummaryText").AsString(int.MaxValue).Nullable().WithDefaultValue(
				"Should there be any queries in relation to this quotation please contact us as soon as possible." + Environment.NewLine +
				"The investigation charge will be applied should the client refuse to go ahead with the specified repairs." + Environment.NewLine +
				"If orders are being sent by email, please use the following address: info.cms@intertek.com");
		}

		public override void Down()
		{
			Delete.Column("QuoteSummaryText").FromTable("CompanyDetails");
		}
	}
}