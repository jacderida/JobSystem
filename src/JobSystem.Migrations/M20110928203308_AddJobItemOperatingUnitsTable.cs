using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928203308)]
	public class M20110928203308_AddJobItemOperatingUnitsTable : Migration
	{
		public override void Up()
		{
			Create.Table("JobItemOperatingUnits")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("JobItemOperatingUnits");
		}
	}
}