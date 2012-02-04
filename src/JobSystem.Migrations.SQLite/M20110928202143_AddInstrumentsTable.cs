using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20110928202143)]
	public class M20110928202143_AddInstrumentsTable : Migration
	{
		public override void Up()
		{
			Create.Table("Instruments")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).Nullable()
				.WithColumn("Manufacturer").AsString(255).Nullable()
				.WithColumn("ModelNo").AsString(255).Nullable()
				.WithColumn("Range").AsString(255).Nullable();
		}

		public override void Down()
		{
			Delete.Table("Instruments");
		}
	}
}