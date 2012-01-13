using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DbWireup.ConsoleRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			var databaseService = new JobSystemDatabaseCreationService("Development");
			databaseService.CreateDatabase(true);
			databaseService.CreateJobSystemSchemaFromMigrations("JobSystem.Migrations.dll");
			databaseService.InitHibernate();
			
			var defaultCurrencyId = Guid.NewGuid();
			var defaultBankDetailsId = Guid.NewGuid();
			var defaultTaxCodeId = Guid.NewGuid();
			var defaultPaymentTermId = Guid.NewGuid();
			var builder = new JobSystemDefaultDataBuilder();
			builder.WithJobTypes("Lab Service", "Field Service")
				.WithJobItemFields("T - Temperature", "E - Electrical", "D - Density", "M - Mechanical", "S - SubContract", "H - Hire", "R - Resale")
				.WithJobItemInitialStatusItems("UKAS Calibration", "House Calibration", "Sub Contract", "Repair")
				.WithJobItemWorkStatusItems("Calibrated", "Repaired", "Failed", "Investigated", "Returned", "Completed", "Reviewed", "Booked In")
				.WithJobItemStatusItems("Quote Prepared", "Quote Accepted", "Quote Rejected", "Consigned", "Order Reviewed", "Ordered", "Delivery Note Produced")
				.WithJobItemWorkTypes("Calibrated", "Repair", "Investigate", "Administration")
				.WithJobItemLocations("Completed", "Calibrated", "Repair", "Sub Contract", "Quote", "Investigated")
				.WithBankDetails(
					new BankDetails { Id = defaultBankDetailsId, Name = "Bank of Scotland", ShortName = "BoS", AccountNo = "00131183", SortCode = "801653", Address1 = "High Street", Address2 = "Johnstone", Iban = "placeholder IBAN"})
				.WithPaymentTerms(Tuple.Create<Guid, string>(defaultPaymentTermId, "30 Days"), Tuple.Create<Guid, string>(Guid.NewGuid(), "As Agreed"))
				.WithCurrencies(Tuple.Create<Guid, string>(defaultCurrencyId, "GBP"), Tuple.Create<Guid, string>(Guid.NewGuid(), "Euros"), Tuple.Create<Guid, string>(Guid.NewGuid(), "Dollars"))
				.WithTaxCodes(
					new TaxCode { Id = Guid.NewGuid(), TaxCodeName = "T0", Rate = 0, Description = "No VAT" },
					new TaxCode { Id = defaultTaxCodeId, TaxCodeName = "T1", Rate = 0.20, Description = "VAT at 20%"})
				.WithEntitySeeds(Tuple.Create<Type, int, string>(typeof(Job), 2000, "JR"));
			var defaultData = builder.Build();
			try
			{
				databaseService.BeginHibernateTransaction();
				databaseService.InsertDefaultData(defaultData);
				databaseService.InsertCompanyDetails(GetCompanyDetails(databaseService, defaultBankDetailsId, defaultCurrencyId, defaultPaymentTermId, defaultTaxCodeId));
				databaseService.InsertAdminUser("admin@intertek.com", "Graham Robertson", "Laboratory Manager", "p'ssw0rd");
				databaseService.CommitHibernateTransaction();
			}
			catch (Exception)
			{
				databaseService.RollbackHibernateTransaction();
			}
		}

		static CompanyDetails GetCompanyDetails(
			JobSystemDatabaseCreationService databaseService, Guid defaultBankDetailsId, Guid defaultCurrencyId, Guid defaultPaymentTermId, Guid defaultTaxCodeId)
		{
			return new CompanyDetails
			{
				Id = Guid.NewGuid(),
				Name = "Intertek EMIS",
				Address1 = "Units 19 & 20 D Wellheads Industrial Centre",
				Address2 = "Wellheads Crescent",
				Address3 = "Dyce",
				Address4 = "Aberdeen",
				Address5 = "AB21 7GA",
				Telephone = "123456",
				Fax = "123456",
				Email = "info@emis-uk.com",
				RegNo = "123456 RegNo",
				VatRegNo = "123456 VatRegNo",
				TermsAndConditions = "placeholder terms and conditions",
				Www = "www.intertek.com",
				DefaultBankDetails = databaseService.GetBankDetails(defaultBankDetailsId),
				DefaultCurrency = databaseService.GetCurrency(defaultCurrencyId),
				DefaultPaymentTerm = databaseService.GetPaymentTerm(defaultPaymentTermId),
				DefaultTaxCode = databaseService.GetTaxCode(defaultTaxCodeId)
			};
		}
	}
}