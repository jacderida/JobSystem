using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120520162219)]
	public class M20120520162219_ChangeCurrencyForeignKeyOnQuotesTable : Migration
	{
		public override void Up()
		{
			Delete.ForeignKey("FK_Quotes_CurrencyId").OnTable("Quotes");
			Create.ForeignKey("FK_Quotes_Currencies").FromTable("Quotes").ForeignColumn("CurrencyId").ToTable("Currencies").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_Quotes_Currencies").OnTable("Quotes");
			Create.ForeignKey("FK_Quotes_CurrencyId").FromTable("Quotes").ForeignColumn("CurrencyId").ToTable("ListItems").PrimaryColumn("Id");
		}
	}
}