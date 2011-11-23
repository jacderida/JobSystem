using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928203114)]
	public class M20110928203114_AddJobCategoriesTable : Migration
	{
		public override void Up()
		{
			Create.Table("JobCategories")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("JobCategories");
		}
	}
}