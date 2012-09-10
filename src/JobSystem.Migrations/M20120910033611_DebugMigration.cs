using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120910033611)]
	public class M20120910033611_DebugMigration : Migration
	{
		// This migration was added to test the build process, nothing more.
		// A follow up migration will remove it.

		public override void Up()
		{
			Create.Table("DebuggingTable").WithColumn("Test").AsString(255).Nullable();
		}

		public override void Down()
		{
			Delete.Table("DebuggingTable");
		}
	}
}