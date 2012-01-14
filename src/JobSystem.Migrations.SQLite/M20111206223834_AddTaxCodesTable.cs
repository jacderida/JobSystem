using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20111206223834)]
	public class M20111206223834_AddTaxCodesTable : Migration
	{
		public override void Up()
		{
			Create.Table("TaxCodes")
				.WithIdColumn()
				.WithColumn("TaxCodeName").AsString(50).NotNullable()
				.WithColumn("Description").AsString(50).NotNullable()
				.WithColumn("Rate").AsDouble().NotNullable();
		}

		public override void Down()
		{
			Delete.Table("TaxCodes");
		}
	}
}