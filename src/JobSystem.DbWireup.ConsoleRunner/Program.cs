using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DbWireup.ConsoleRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			var databaseService = new JobSystemDatabaseCreationService("JobSystemDatabase", "Development");
			databaseService.CreateDatabase(true);
			databaseService.CreateJobSystemSchemaFromMigrations("JobSystem.Migrations.dll");
			databaseService.InitHibernate();
			
			var defaultCurrencyId = Guid.NewGuid();
			var defaultBankDetailsId = Guid.NewGuid();
			var defaultTaxCodeId = Guid.NewGuid();
			var defaultPaymentTermId = Guid.NewGuid();
			var builder = new JobSystemDefaultDataBuilder();
			builder
				.WithJobTypes(
					Tuple.Create<string, ListItemType>("Lab Service", ListItemType.JobTypeField),
					Tuple.Create<string, ListItemType>("Field Service", ListItemType.JobTypeService))
				.WithListItemCategories(
					Tuple.Create<string, ListItemCategoryType>("Job Type", ListItemCategoryType.JobType),
					Tuple.Create<string, ListItemCategoryType>("Category", ListItemCategoryType.JobItemCategory),
					Tuple.Create<string, ListItemCategoryType>("Initial Status", ListItemCategoryType.JobItemInitialStatus),
					Tuple.Create<string, ListItemCategoryType>("Status", ListItemCategoryType.JobItemStatus),
					Tuple.Create<string, ListItemCategoryType>("Location", ListItemCategoryType.JobItemLocation),
					Tuple.Create<string, ListItemCategoryType>("Work Type", ListItemCategoryType.JobItemWorkType),
					Tuple.Create<string, ListItemCategoryType>("Status", ListItemCategoryType.JobItemWorkStatus),
					Tuple.Create<string, ListItemCategoryType>("Currency", ListItemCategoryType.Currency),
					Tuple.Create<string, ListItemCategoryType>("Payment Term", ListItemCategoryType.PaymentTerm),
					Tuple.Create<string, ListItemCategoryType>("Initial Location", ListItemCategoryType.JobItemInitialLocation))
				.WithJobItemCategories(
					Tuple.Create<string, ListItemType>("T - Temperature", ListItemType.CategoryTemperature),
					Tuple.Create<string, ListItemType>("E - Electrical", ListItemType.CategoryElectrical),
					Tuple.Create<string, ListItemType>("D - Density", ListItemType.CategoryDensity),
					Tuple.Create<string, ListItemType>("M - Mechanical", ListItemType.CategoryMechanical),
					Tuple.Create<string, ListItemType>("S - SubContract", ListItemType.CategorySubContract),
					Tuple.Create<string, ListItemType>("H - Hire", ListItemType.CategoryHire),
					Tuple.Create<string, ListItemType>("R - Resale", ListItemType.CategoryResale),
					Tuple.Create<string, ListItemType>("F - Field", ListItemType.CategoryField),
					Tuple.Create<string, ListItemType>("P - Pressure", ListItemType.CategoryPressure))
				.WithJobItemInitialStatusItems(
					Tuple.Create<string, ListItemType>("UKAS Calibration", ListItemType.InitialStatusUkasCalibration),
					Tuple.Create<string, ListItemType>("House Calibration", ListItemType.InitialStatusHouseCalibration),
					Tuple.Create<string, ListItemType>("Sub Contract", ListItemType.InitialStatusSubContract),
					Tuple.Create<string, ListItemType>("Repair", ListItemType.InitialStatusRepair))
				.WithJobItemWorkStatusItems(
					Tuple.Create<string, ListItemType>("Calibrated", ListItemType.WorkStatusCalibrated),
					Tuple.Create<string, ListItemType>("Repaired", ListItemType.WorkStatusRepaired),
					Tuple.Create<string, ListItemType>("Failed", ListItemType.WorkStatusFailed),
					Tuple.Create<string, ListItemType>("Investigated", ListItemType.WorkStatusInvestigated),
					Tuple.Create<string, ListItemType>("Returned", ListItemType.WorkStatusReturned),
					Tuple.Create<string, ListItemType>("Completed", ListItemType.WorkStatusCompleted),
					Tuple.Create<string, ListItemType>("Reviewed", ListItemType.WorkStatusReviewed))
				.WithJobItemStatusItems(
					Tuple.Create<string, ListItemType>("Quote Prepared", ListItemType.StatusQuotedPrepared),
					Tuple.Create<string, ListItemType>("Quote Accepted", ListItemType.StatusQuoteAccepted),
					Tuple.Create<string, ListItemType>("Quote Rejected", ListItemType.StatusQuoteRejected),
					Tuple.Create<string, ListItemType>("Consigned", ListItemType.StatusConsigned),
					Tuple.Create<string, ListItemType>("Order Reviewed", ListItemType.StatusOrderReviewed),
					Tuple.Create<string, ListItemType>("Ordered", ListItemType.StatusOrdered),
					Tuple.Create<string, ListItemType>("Delivery Note Produced", ListItemType.StatusDeliveryNoteProduced),
					Tuple.Create<string, ListItemType>("Booked In", ListItemType.StatusBookedIn),
					Tuple.Create<string, ListItemType>("Invoiced", ListItemType.StatusInvoiced))
				.WithJobItemWorkTypes(
					Tuple.Create<string, ListItemType>("Calibration", ListItemType.WorkTypeCalibration),
					Tuple.Create<string, ListItemType>("Repair", ListItemType.WorkTypeRepair),
					Tuple.Create<string, ListItemType>("Investigation", ListItemType.WorkTypeInvestigation),
					Tuple.Create<string, ListItemType>("Administration", ListItemType.WorkTypeAdministration))
				.WithJobItemLocations(
					Tuple.Create<string, ListItemType>("Completed", ListItemType.WorkLocationCompleted),
					Tuple.Create<string, ListItemType>("Calibrated", ListItemType.WorkLocationCalibrated),
					Tuple.Create<string, ListItemType>("Repaired", ListItemType.WorkLocationRepaired),
					Tuple.Create<string, ListItemType>("Sub Contract", ListItemType.WorkLocationSubContract),
					Tuple.Create<string, ListItemType>("Quoted", ListItemType.WorkLocationQuoted),
					Tuple.Create<string, ListItemType>("Investigated", ListItemType.WorkLocationInvestigated))
				.WithBankDetails(
					new BankDetails { Id = defaultBankDetailsId, Name = "Bank of Scotland", ShortName = "BoS", AccountNo = "00131183", SortCode = "801653", Address1 = "High Street", Address2 = "Johnstone", Iban = "placeholder IBAN"})
				.WithPaymentTerms(
					Tuple.Create<Guid, string, ListItemType>(defaultPaymentTermId, "30 Days", ListItemType.PaymentTerm30Days),
					Tuple.Create<Guid, string, ListItemType>(Guid.NewGuid(), "As Agreed", ListItemType.PaymentTermAsAgreed))
				.WithCurrencies(
					Tuple.Create<Guid, string, ListItemType>(defaultCurrencyId, "GBP", ListItemType.CurrencyGbp),
					Tuple.Create<Guid, string, ListItemType>(Guid.NewGuid(), "Euros", ListItemType.CurrencyUsd),
					Tuple.Create<Guid, string, ListItemType>(Guid.NewGuid(), "Dollars", ListItemType.CurrencyEuro))
				.WithTaxCodes(
					new TaxCode { Id = Guid.NewGuid(), TaxCodeName = "T0", Rate = 0, Description = "No VAT" },
					new TaxCode { Id = defaultTaxCodeId, TaxCodeName = "T1", Rate = 0.20, Description = "VAT at 20%"})
				.WithEntitySeeds(
					Tuple.Create<Type, int, string>(typeof(Job), 2000, "JR"),
					Tuple.Create<Type, int, string>(typeof(Consignment), 2000, "CR"));
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