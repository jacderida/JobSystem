using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
	[Migration(20111208212831)]
	public class M20111208212831_AddCompanyDetailsTable : Migration
	{
		public override void Up()
		{
			Create.Table("CompanyDetails")
				.WithIdColumn()
				.WithColumn("Name").AsString(255).NotNullable()
				.WithColumn("Address1").AsString(255).Nullable()
				.WithColumn("Address2").AsString(255).Nullable()
				.WithColumn("Address3").AsString(255).Nullable()
				.WithColumn("Address4").AsString(255).Nullable()
				.WithColumn("Address5").AsString(255).Nullable()
				.WithColumn("Telephone").AsString(50).NotNullable()
				.WithColumn("Fax").AsString(50).NotNullable()
				.WithColumn("RegNo").AsString(50).NotNullable()
				.WithColumn("VatRegNo").AsString(50).NotNullable()
				.WithColumn("Email").AsString(50).NotNullable()
				.WithColumn("Www").AsString(255).NotNullable()
				.WithColumn("TermsAndConditions").AsString(20000).NotNullable()
				.WithColumn("DefaultCurrencyId").AsGuid().NotNullable()
				.WithColumn("DefaultTaxCodeId").AsGuid().NotNullable()
				.WithColumn("DefaultPaymentTermId").AsGuid().NotNullable()
				.WithColumn("DefaultBankDetailsId").AsGuid().NotNullable()
				.WithColumn("MainLogo").AsBinary(20000).Nullable()
				.WithColumn("ApplyAllPrices").AsBoolean().NotNullable();
			Create.ForeignKey("FK_CompanyDetails_Currencies")
				.FromTable("CompanyDetails")
				.ForeignColumn("DefaultCurrencyId")
				.ToTable("Currencies")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_CompanyDetails_TaxCodes")
				.FromTable("CompanyDetails")
				.ForeignColumn("DefaultTaxCodeId")
				.ToTable("TaxCodes")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_CompanyDetails_PaymentTerms")
				.FromTable("CompanyDetails")
				.ForeignColumn("DefaultPaymentTermId")
				.ToTable("ListItems")
				.PrimaryColumn("Id");
			Create.ForeignKey("FK_CompanyDetails_BankDetails")
				.FromTable("CompanyDetails")
				.ForeignColumn("DefaultBankDetailsId")
				.ToTable("BankDetails")
				.PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.Table("CompanyDetails");
		}
	}
}