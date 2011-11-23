using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928203401)]
	public class M20110928203401_AddJobItemWorkTypesTable : Migration
	{
		public override void Up()
		{
			Create.Table("JobItemWorkTypes")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("JobItemWorkTypes");
		}
	}
}