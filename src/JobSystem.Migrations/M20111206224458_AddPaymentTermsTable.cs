using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20111206224458)]
	public class M20111206224458_AddPaymentTermsTable : Migration
	{
		public override void Up()
		{
			Create.Table("PaymentTerms")
				.WithIdColumn()
				.WithColumn("Name").AsString(50).NotNullable();
		}

		public override void Down()
		{
			Delete.Table("PaymentTerms");
		}
	}
}