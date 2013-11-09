using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using JobSystem.Admin.DbWireup;
using JobSystem.DataModel.Entities;

namespace JobSystem.DbWireup.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseService = new JobSystemDatabaseCreationService(ConfigurationManager.AppSettings["TenantName"]);
            databaseService.CreateDatabase(true);
            databaseService.CreateServerLogin();
            databaseService.CreateUserLogin();
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
                    Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.WorkTypeId, "Work Type", ListItemCategoryType.JobItemWorkType),
                    Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.WorkStatusId, "Status", ListItemCategoryType.JobItemWorkStatus),
                    Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.PaymentTermId, "Payment Term", ListItemCategoryType.PaymentTerm),
                    Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.CertificateId, "Certificate", ListItemCategoryType.Certificate))
                .WithJobTypes(
                    Tuple.Create<string, ListItemType, Guid>("Lab Service", ListItemType.JobTypeField, ListCategoryIds.JobTypeId),
                    Tuple.Create<string, ListItemType, Guid>("Site Service", ListItemType.JobTypeService, ListCategoryIds.JobTypeId))
                .WithCertificateTypes(
                    Tuple.Create<string, ListItemType, Guid>("House", ListItemType.CertificateTypeHouse, ListCategoryIds.CertificateId),
                    Tuple.Create<string, ListItemType, Guid>("UKAS", ListItemType.CertificateTypeUkas, ListCategoryIds.CertificateId))
                .WithJobItemCategories(
                    Tuple.Create<string, ListItemType, Guid>("T - Temperature", ListItemType.CategoryTemperature, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("E - Electrical", ListItemType.CategoryElectrical, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("D - Density", ListItemType.CategoryDensity, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("M - Mechanical", ListItemType.CategoryMechanical, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("O - Outsource", ListItemType.CategorySubContract, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("H - Hire", ListItemType.CategoryHire, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("R - Resale", ListItemType.CategoryResale, ListCategoryIds.CategoryId),
                    Tuple.Create<string, ListItemType, Guid>("S - Site Services", ListItemType.CategoryField, ListCategoryIds.CategoryId),
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
                    Tuple.Create<string, ListItemType, Guid>("Item with Sub Contractor", ListItemType.StatusItemWithSubContractor, ListCategoryIds.StatusId),
                    Tuple.Create<string, ListItemType, Guid>("Awaiting Parts", ListItemType.StatusAwaitingParts, ListCategoryIds.StatusId),
                    Tuple.Create<string, ListItemType, Guid>("Delivery Note Produced", ListItemType.StatusDeliveryNoteProduced, ListCategoryIds.StatusId),
                    Tuple.Create<string, ListItemType, Guid>("Booked In", ListItemType.StatusBookedIn, ListCategoryIds.StatusId),
                    Tuple.Create<string, ListItemType, Guid>("Invoiced", ListItemType.StatusInvoiced, ListCategoryIds.StatusId),
                    Tuple.Create<string, ListItemType, Guid>("Parts Received", ListItemType.StatusPartsReceived, ListCategoryIds.StatusId),
                    Tuple.Create<string, ListItemType, Guid>("Quoted", ListItemType.StatusQuoted, ListCategoryIds.StatusId))
                .WithJobItemWorkTypes(
                    Tuple.Create<string, ListItemType, Guid>("Calibration", ListItemType.WorkTypeCalibration, ListCategoryIds.WorkTypeId),
                    Tuple.Create<string, ListItemType, Guid>("Repair", ListItemType.WorkTypeRepair, ListCategoryIds.WorkTypeId),
                    Tuple.Create<string, ListItemType, Guid>("Investigation", ListItemType.WorkTypeInvestigation, ListCategoryIds.WorkTypeId),
                    Tuple.Create<string, ListItemType, Guid>("Administration", ListItemType.WorkTypeAdministration, ListCategoryIds.WorkTypeId))
                .WithBankDetails(
                    new BankDetails { Id = defaultBankDetailsId, Name = "Bank of Scotland", ShortName = "BoS", AccountNo = "00131183", SortCode = "801653", Address1 = "High Street", Address2 = "Johnstone", Iban = "placeholder IBAN" })
                .WithPaymentTerms(
                    Tuple.Create<Guid, string, ListItemType, Guid>(defaultPaymentTermId, "30 Days", ListItemType.PaymentTerm30Days, ListCategoryIds.PaymentTermId),
                    Tuple.Create<Guid, string, ListItemType, Guid>(Guid.NewGuid(), "As Agreed", ListItemType.PaymentTermAsAgreed, ListCategoryIds.PaymentTermId))
                .WithCurrencies(
                    new Currency { Id = defaultCurrencyId, Name = "GBP", DisplayMessage = "All prices in GBP" },
                    new Currency { Id = Guid.NewGuid(), Name = "EUR", DisplayMessage = "All prices in Euros" },
                    new Currency { Id = Guid.NewGuid(), Name = "USD", DisplayMessage = "All prices in Dollars" })
                .WithTaxCodes(
                    new TaxCode { Id = Guid.NewGuid(), TaxCodeName = "T0", Rate = 0, Description = "No VAT" },
                    new TaxCode { Id = defaultTaxCodeId, TaxCodeName = "T1", Rate = 0.20, Description = "VAT at 20%" })
                .WithEntitySeeds(
                    Tuple.Create<Type, int, string>(typeof(Job), 2000, "JR"),
                    Tuple.Create<Type, int, string>(typeof(Consignment), 2000, "CR"),
                    Tuple.Create<Type, int, string>(typeof(Quote), 2000, "QR"),
                    Tuple.Create<Type, int, string>(typeof(Order), 2000, "OR"),
                    Tuple.Create<Type, int, string>(typeof(Certificate), 2000, "CERT"),
                    Tuple.Create<Type, int, string>(typeof(Delivery), 2000, "DR"),
                    Tuple.Create<Type, int, string>(typeof(Invoice), 2000, "IR"));
            var defaultData = builder.Build();
            try
            {
                databaseService.BeginHibernateTransaction();
                databaseService.InsertDefaultData(defaultData);
                databaseService.InsertCompanyDetails(GetCompanyDetails(databaseService, defaultBankDetailsId, defaultCurrencyId, defaultPaymentTermId, defaultTaxCodeId));
                databaseService.InsertAdminUser("admin@intertek.com", "Graham Robertson", "Laboratory Manager", "p'ssw0rd");
                databaseService.CommitHibernateTransaction();
                Console.WriteLine(databaseService.GetGeneratedPassword());
                Console.WriteLine("Copy generated password then press any key to quit.");
                Console.ReadKey();
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
            var logoImage = Image.FromStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("JobSystem.DbWireup.ConsoleRunner.intertek_logon.gif"));
            var ms = new MemoryStream();
            logoImage.Save(ms, logoImage.RawFormat);
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
                DefaultTaxCode = databaseService.GetTaxCode(defaultTaxCodeId),
                DefaultCultureCode = "en-GB",
                MainLogo = ms.ToArray(),
				OrderAcknowledgeText = "Can you acknowledge this order and advise of a delivery date as soon as possible? The delivery address is as stated below, unless you are advised otherwise.",
				QuoteSummaryText = 
					"Should there be any queries in relation to this quotation please contact us as soon as possible." + Environment.NewLine +
					"The investigation charge will be applied should the client refuse to go ahead with the specified repairs." + Environment.NewLine +
					"If orders are being sent by email, please use the following address: info.cms@intertek.com"
            };
        }
    }

    public static class ListCategoryIds
    {
        public static Guid JobTypeId = Guid.NewGuid();
        public static Guid CategoryId = Guid.NewGuid();
        public static Guid InitialStatusId = Guid.NewGuid();
        public static Guid StatusId = Guid.NewGuid();
        public static Guid WorkTypeId = Guid.NewGuid(); 
        public static Guid WorkStatusId = Guid.NewGuid();
        public static Guid PaymentTermId = Guid.NewGuid();
        public static Guid CertificateId = Guid.NewGuid();
    }
}