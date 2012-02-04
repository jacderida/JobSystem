using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20110928202539)]
	public class M20110928202539_AddUserAccountsTable : Migration
	{
		public override void Up()
		{
			Create.Table("UserAccounts")
				.WithIdColumn()
				.WithColumn("EmailAddress").AsString(255).NotNullable().Unique()
				.WithColumn("Name").AsString(255).NotNullable()
				.WithColumn("JobTitle").AsString(255).NotNullable()
				.WithColumn("PasswordHash").AsString(255).NotNullable()
				.WithColumn("PasswordSalt").AsString(255).NotNullable()
				.WithColumn("Roles").AsInt32().NotNullable();
		}

		public override void Down()
		{
			Delete.Table("UserAccounts");
		}
	}
}