using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20110928202938)]
	public class M20110928202938_AddStatusTable : Migration
	{
		public override void Up()
		{
			Create.Table("Status")
				.WithIdColumn()
				.WithColumn("Description").AsString(255).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("Status");
		}
	}
}