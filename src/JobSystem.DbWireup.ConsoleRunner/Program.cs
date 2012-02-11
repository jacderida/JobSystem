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
				.WithListItemCategories(
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.JobTypeId, "Job Type", ListItemCategoryType.JobType),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.CategoryId, "Category", ListItemCategoryType.JobItemCategory),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.InitialStatusId, "Initial Status", ListItemCategoryType.JobItemInitialStatus),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.StatusId, "Status", ListItemCategoryType.JobItemStatus),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.LocationId, "Location", ListItemCategoryType.JobItemLocation),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.WorkTypeId, "Work Type", ListItemCategoryType.JobItemWorkType),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.WorkStatusId, "Status", ListItemCategoryType.JobItemWorkStatus),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.CurrencyId, "Currency", ListItemCategoryType.Currency),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.PaymentTermId, "Payment Term", ListItemCategoryType.PaymentTerm),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.InitialLocationId, "Initial Location", ListItemCategoryType.JobItemInitialLocation))
				.WithJobTypes(
					Tuple.Create<string, ListItemType, Guid>("Lab Service", ListItemType.JobTypeField, ListCategoryIds.JobTypeId),
					Tuple.Create<string, ListItemType, Guid>("Field Service", ListItemType.JobTypeService, ListCategoryIds.JobTypeId))
				.WithJobItemCategories(
					Tuple.Create<string, ListItemType, Guid>("T - Temperature", ListItemType.CategoryTemperature, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("E - Electrical", ListItemType.CategoryElectrical, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("D - Density", ListItemType.CategoryDensity, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("M - Mechanical", ListItemType.CategoryMechanical, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("S - SubContract", ListItemType.CategorySubContract, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("H - Hire", ListItemType.CategoryHire, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("R - Resale", ListItemType.CategoryResale, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("F - Field", ListItemType.CategoryField, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("P - Pressure", ListItemType.CategoryPressure, ListCategoryIds.CategoryId))
				.WithJobItemInitialStatusItems(
					Tuple.Create<string, ListItemType, Guid>("UKAS Calibration", ListItemType.InitialStatusUkasCalibration, ListCategoryIds.InitialStatusId),
					Tuple.Create<string, ListItemType, Guid>("House Calibration", ListItemType.InitialStatusHouseCalibration, ListCategoryIds.InitialStatusId),
					Tuple.Create<string, ListItemType, Guid>("Sub Contract", ListItemType.InitialStatusSubContract, ListCategoryIds.InitialStatusId),
					Tuple.Create<string, ListItemType, Guid>("Repair", ListItemType.InitialStatusRepair, ListCategoryIds.InitialStatusId))
				.WithJobItemWorkStatusItems(
					Tuple.Create<string, ListItemType, Guid>("Calibrated", ListItemType.WorkStatusCalibrated, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Repaired", ListItemType.WorkStatusRepaired, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Failed", ListItemType.WorkStatusFailed, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Investigated", ListItemType.WorkStatusInvestigated, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Returned", ListItemType.WorkStatusReturned, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Completed", ListItemType.WorkStatusCompleted, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Reviewed", ListItemType.WorkStatusReviewed, ListCategoryIds.WorkStatusId))
				.WithJobItemStatusItems(
					Tuple.Create<string, ListItemType, Guid>("Quote Prepared", ListItemType.StatusQuotedPrepared, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Quote Accepted", ListItemType.StatusQuoteAccepted, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Quote Rejected", ListItemType.StatusQuoteRejected, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Consigned", ListItemType.StatusConsigned, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Order Reviewed", ListItemType.StatusOrderReviewed, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Ordered", ListItemType.StatusOrdered, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Delivery Note Produced", ListItemType.StatusDeliveryNoteProduced, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Booked In", ListItemType.StatusBookedIn, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Invoiced", ListItemType.StatusInvoiced, ListCategoryIds.StatusId))
				.WithJobItemWorkTypes(
					Tuple.Create<string, ListItemType, Guid>("Calibration", ListItemType.WorkTypeCalibration, ListCategoryIds.WorkTypeId),
					Tuple.Create<string, ListItemType, Guid>("Repair", ListItemType.WorkTypeRepair, ListCategoryIds.WorkTypeId),
					Tuple.Create<string, ListItemType, Guid>("Investigation", ListItemType.WorkTypeInvestigation, ListCategoryIds.WorkTypeId),
					Tuple.Create<string, ListItemType, Guid>("Administration", ListItemType.WorkTypeAdministration, ListCategoryIds.WorkTypeId))
				.WithJobItemLocations(
					Tuple.Create<string, ListItemType, Guid>("Completed", ListItemType.WorkLocationCompleted, ListCategoryIds.LocationId),
					Tuple.Create<string, ListItemType, Guid>("Calibrated", ListItemType.WorkLocationCalibrated, ListCategoryIds.LocationId),
					Tuple.Create<string, ListItemType, Guid>("Repaired", ListItemType.WorkLocationRepaired, ListCategoryIds.LocationId),
					Tuple.Create<string, ListItemType, Guid>("Sub Contract", ListItemType.WorkLocationSubContract, ListCategoryIds.LocationId),
					Tuple.Create<string, ListItemType, Guid>("Quoted", ListItemType.WorkLocationQuoted, ListCategoryIds.LocationId),
					Tuple.Create<string, ListItemType, Guid>("Investigated", ListItemType.WorkLocationInvestigated, ListCategoryIds.LocationId))
				.WithJobItemInitialLocations(
					Tuple.Create<string, ListItemType, Guid>("House Calibration", ListItemType.InitialWorkLocationHouseCalibration, ListCategoryIds.InitialLocationId),
					Tuple.Create<string, ListItemType, Guid>("UKAS Calibration", ListItemType.InitialWorkLocationUkasCalibration, ListCategoryIds.InitialLocationId),
					Tuple.Create<string, ListItemType, Guid>("Repair", ListItemType.InitialWorkLocationRepair, ListCategoryIds.InitialLocationId),
					Tuple.Create<string, ListItemType, Guid>("Sub Contract", ListItemType.InitialWorkLocationSubContract, ListCategoryIds.InitialLocationId),
					Tuple.Create<string, ListItemType, Guid>("Site", ListItemType.InitialWorkLocationSite, ListCategoryIds.InitialLocationId))
				.WithBankDetails(
					new BankDetails { Id = defaultBankDetailsId, Name = "Bank of Scotland", ShortName = "BoS", AccountNo = "00131183", SortCode = "801653", Address1 = "High Street", Address2 = "Johnstone", Iban = "placeholder IBAN" })
				.WithPaymentTerms(
					Tuple.Create<Guid, string, ListItemType, Guid>(defaultPaymentTermId, "30 Days", ListItemType.PaymentTerm30Days, ListCategoryIds.PaymentTermId),
					Tuple.Create<Guid, string, ListItemType, Guid>(Guid.NewGuid(), "As Agreed", ListItemType.PaymentTermAsAgreed, ListCategoryIds.PaymentTermId))
				.WithCurrencies(
					Tuple.Create<Guid, string, ListItemType, Guid>(defaultCurrencyId, "GBP", ListItemType.CurrencyGbp, ListCategoryIds.CurrencyId),
					Tuple.Create<Guid, string, ListItemType, Guid>(Guid.NewGuid(), "Euros", ListItemType.CurrencyUsd, ListCategoryIds.CurrencyId),
					Tuple.Create<Guid, string, ListItemType, Guid>(Guid.NewGuid(), "Dollars", ListItemType.CurrencyEuro, ListCategoryIds.CurrencyId))
				.WithTaxCodes(
					new TaxCode { Id = Guid.NewGuid(), TaxCodeName = "T0", Rate = 0, Description = "No VAT" },
					new TaxCode { Id = defaultTaxCodeId, TaxCodeName = "T1", Rate = 0.20, Description = "VAT at 20%" })
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
				throw;
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

	public static class ListCategoryIds
	{
		public static Guid JobTypeId = Guid.NewGuid();
		public static Guid CategoryId = Guid.NewGuid();
		public static Guid InitialStatusId = Guid.NewGuid();
		public static Guid StatusId = Guid.NewGuid();
		public static Guid LocationId = Guid.NewGuid();
		public static Guid WorkTypeId = Guid.NewGuid(); 
		public static Guid WorkStatusId = Guid.NewGuid();
		public static Guid CurrencyId = Guid.NewGuid();
		public static Guid PaymentTermId = Guid.NewGuid();
		public static Guid InitialLocationId = Guid.NewGuid();
	}
}