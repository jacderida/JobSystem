using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120519163128)]
	public class M20120519163128_AddCurrencyTable : Migration
	{
		public override void Up()
		{
			Create.Table("Currencies")
				.WithIdColumn()
				.WithColumn("Name").AsString(50).NotNullable()
				.WithColumn("DisplayMessage").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("Currencies");
		}
	}
}