using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20111206224230)]
	public class M20111206224230_AddCurrenciesTable : Migration
	{
		public override void Up()
		{
			Create.Table("Currencies")
				.WithIdColumn()
				.WithColumn("Name").AsString(50).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("Currencies");
		}
	}
}