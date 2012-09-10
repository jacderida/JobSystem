using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120910042017)]
	public class M20120910042017_RemoveDebugMigration : Migration
	{
		public override void Up()
		{
			Delete.Table("DebuggingTable");
		}

		public override void Down()
		{
			Create.Table("DebuggingTable").WithColumn("Test").AsString(255).Nullable();
		}
	}
}