using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928203209)]
	public class M20110928203209_AddJobItemWorkLocationsTable : Migration
	{
		public override void Up()
		{
			Create.Table("JobItemWorkLocations")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("JobItemWorkLocations");
		}
	}
}