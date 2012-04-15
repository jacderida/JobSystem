using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
	[Migration(20120405211354)]
	public class M20120405211354_AddInvoicesTable : Migration
	{
		public override void Up()
		{
			Create.Table("Invoices")
				.WithIdColumn()
				.WithColumn("InvoiceNumber").AsString(50).Unique().NotNullable()
				.WithColumn("DateCreated").AsDateTime().NotNullable()
				.WithColumn("OrderNo").AsString(50).Nullable()
				.WithColumn("CurrencyId").AsGuid().NotNullable()
				.WithColumn("CustomerId").AsGuid().NotNullable()
				.WithColumn("BankDetailsId").AsGuid().NotNullable()
				.WithColumn("PaymentTermId").AsGuid().NotNullable()
				.WithColumn("TaxCodeId").AsGuid().NotNullable();
			Create.ForeignKey("FK_Invoices_Currency").FromTable("Invoices").ForeignColumn("CurrencyId").ToTable("ListItems").PrimaryColumn("Id");
			Create.ForeignKey("FK_Invoices_Customer").FromTable("Invoices").ForeignColumn("CustomerId").ToTable("Customers").PrimaryColumn("Id");
			Create.ForeignKey("FK_Invoices_BankDetails").FromTable("Invoices").ForeignColumn("BankDetailsId").ToTable("BankDetails").PrimaryColumn("Id");
			Create.ForeignKey("FK_Invoices_PaymentTerm").FromTable("Invoices").ForeignColumn("PaymentTermId").ToTable("ListItems").PrimaryColumn("Id");
			Create.ForeignKey("FK_Invoices_TaxCode").FromTable("Invoices").ForeignColumn("TaxCodeId").ToTable("TaxCodes").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK_Invoices_Currency").OnTable("Invoices");
			Delete.ForeignKey("FK_Invoices_Customer").OnTable("Invoices");
			Delete.ForeignKey("FK_Invoices_BankDetails").OnTable("Invoices");
			Delete.ForeignKey("FK_Invoices_PaymentTerm").OnTable("Invoices");
			Delete.ForeignKey("FK_Invoices_TaxCode").OnTable("Invoices");
			Delete.Table("Invoices");
		}
	}
}