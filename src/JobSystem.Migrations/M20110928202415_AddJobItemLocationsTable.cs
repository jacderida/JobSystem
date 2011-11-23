using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928202415)]
	public class M20110928202415_AddJobItemLocationsTable : Migration
	{
		public override void Up()
		{
			Create.Table("JobItemLocations")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("JobItemLocations");
		}
	}
}