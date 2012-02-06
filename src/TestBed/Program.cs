using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Mappings;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.DbWireup;
using JobSystem.Framework.Messaging;
using JobSystem.Queueing.Msmq;
using NHibernate.Connection;
using JobSystem.Framework.Security;

namespace TestBed
{
	public class Program
	{
		static void Main(string[] args)
		{
			CreateDatabase();
			InitialiseNHibernateSessions();
			
			var queueDispatcher = new MsmqQueueGateway<IMessage>("", "");
			var initialUserContext = new TestUserContext(TestUserContext.CreateAdminUser());
			var userManagementService = new UserManagementService(initialUserContext, new UserAccountRepository(), new CryptographicService(), queueDispatcher);
			var adminUser = userManagementService.GetUsers().Where(u => u.EmailAddress == "admin@intertek.com").Single();

			var testUserContext = new TestUserContext(adminUser);
			var currentUser = testUserContext.GetCurrentUser();
			var listItemService = new ListItemService(testUserContext, new ListItemRepository(), queueDispatcher);
			var customerService = new CustomerService(testUserContext, new CustomerRepository(), queueDispatcher);
			var jobService = new JobService(
				testUserContext, new JobRepository(), new ListItemRepository(), new CustomerRepository(), new EntityIdProvider(), queueDispatcher);
			var jobItemService = new JobItemService(
				testUserContext, new JobRepository(), new JobItemRepository(),
				new ListItemService(testUserContext, new ListItemRepository(), queueDispatcher),
				new InstrumentService(testUserContext, new InstrumentRepository(), queueDispatcher),
				queueDispatcher);
			var instrumentService = new InstrumentService(testUserContext, new InstrumentRepository(), queueDispatcher);
			var itemHistoryService = new ItemHistoryService(testUserContext, new JobItemRepository(), new ListItemRepository(), queueDispatcher);

			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();

			NHibernateSession.Current.BeginTransaction();
			customerService.Create(customerId, "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());
			instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Digital Pressure Indicator");
			jobService.CreateJob(jobId, "job instructions", "ORDER12345", "ADVICE12345", listItemService.GetAllByType(ListItemType.JobType).First().Id, customerId, "job notes", "job contact");
			jobItemService.CreateJobItem(
				jobId, jobItemId, instrumentId, "12345", "AST12345", listItemService.GetAllByType(ListItemType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByType(ListItemType.JobItemLocation).First().Id, listItemService.GetAllByType(ListItemType.JobItemField).First().Id, 12,
				"job instructions", "accessories", false, String.Empty, "comments");
			itemHistoryService.CreateItemHistory(
				Guid.NewGuid(), jobItemId, 12, 12, "blah",
				listItemService.GetAllByType(ListItemType.JobItemWorkStatus).First().Id,
				listItemService.GetAllByType(ListItemType.JobItemWorkType).First().Id,
				listItemService.GetAllByType(ListItemType.JobItemLocation).First().Id);
			NHibernateSession.Current.Transaction.Commit();
		}

		private static void InitialiseNHibernateSessions()
		{
			NHibernateSession.Reset();
			SimpleConnectionProvider.CatalogName = "JobSystem.Development";
			var dbConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(NHibernateSession.GetInitialConnectionString()).Provider<SimpleConnectionProvider>();
			NHibernateSession.Init(
				new SimpleSessionStorage(), new[] { "JobSystem.DataAccess.NHibernate.dll" }, new AutoPersistenceModelGenerator().Generate(), null, null, null, dbConfigurer);
		}

		private static void CreateDatabase()
		{
			var databaseService = new JobSystemDatabaseCreationService("InitialDatabase", "Development");
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
				.WithJobItemWorkStatusItems("Calibrated", "Repaired", "Failed", "Investigated", "Returned", "Completed", "Reviewed")
				.WithJobItemStatusItems("Quote Prepared", "Quote Accepted", "Quote Rejected", "Consigned", "Order Reviewed", "Ordered", "Delivery Note Produced", "Booked In")
				.WithJobItemWorkTypes("Calibration", "Repair", "Investigation", "Administration")
				.WithJobItemLocations("Completed", "Calibrated", "Repaired", "Sub Contract", "Quoted", "Investigated")
				.WithBankDetails(
					new BankDetails { Id = defaultBankDetailsId, Name = "Bank of Scotland", ShortName = "BoS", AccountNo = "00131183", SortCode = "801653", Address1 = "High Street", Address2 = "Johnstone", Iban = "placeholder IBAN" })
				.WithPaymentTerms(Tuple.Create<Guid, string>(defaultPaymentTermId, "30 Days"), Tuple.Create<Guid, string>(Guid.NewGuid(), "As Agreed"))
				.WithCurrencies(Tuple.Create<Guid, string>(defaultCurrencyId, "GBP"), Tuple.Create<Guid, string>(Guid.NewGuid(), "Euros"), Tuple.Create<Guid, string>(Guid.NewGuid(), "Dollars"))
				.WithTaxCodes(
					new TaxCode { Id = Guid.NewGuid(), TaxCodeName = "T0", Rate = 0, Description = "No VAT" },
					new TaxCode { Id = defaultTaxCodeId, TaxCodeName = "T1", Rate = 0.20, Description = "VAT at 20%" })
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

		private static CompanyDetails GetCompanyDetails(
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

	public class SimpleConnectionProvider : DriverConnectionProvider
	{
		public static string CatalogName { get; set; }

		protected override string ConnectionString
		{
			get
			{
				var csb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["JobSystemDatabase"].ConnectionString);
				csb.InitialCatalog = CatalogName;
				return csb.ToString();
			}
		}
	}

	public class TestUserContext : IUserContext
	{
		private readonly UserAccount _userAccount;

		public TestUserContext()
			: this(UserAccount.Anonymous)
		{
		}

		public TestUserContext(UserAccount userAccount)
		{
			_userAccount = userAccount;
		}

		public TestUserContext(Guid userId, string emailAddress, string userName, string jobTitle, UserRole roles)
			: this(new UserAccount()
			{
				Id = userId,
				Name = userName,
				JobTitle = jobTitle,
				EmailAddress = emailAddress,
				Roles = roles
			})
		{
		}

		public static IUserContext Create(string emailAddress, string userName, string jobTitle, UserRole roles)
		{
			return new TestUserContext(Guid.Empty, emailAddress, userName, jobTitle, roles);
		}

		public static UserAccount CreateAdminUser()
		{
			return new UserAccount
			{
				Id = Guid.Parse("A48B14F6-B643-4F5D-97AF-1DEC23CEB3EA"),
				EmailAddress = "admin@intertek.com",
				Name = "Graham Robertson",
				JobTitle = "Laboratory Manager",
				PasswordHash = "S3naGkYpGgEqG77JeD6pvkN8AtVUmkg0Hx5PUfP3ixX834pnYwj6hBDoY49FP+lgAEwhXHYkSjoLPVoze3Ln+Q==",
				PasswordSalt = "K5fSyL55UIUN1/YyZMIMz/YElG4b1K6oUGsKZn/UC97/wtBP1O9JUJnAcxjergpNEldX3EY4udXdkpJSGpclCw==",
				Roles = UserRole.Admin
			};
		}

		public UserAccount GetCurrentUser()
		{
			return _userAccount;
		}
	}

	public class EntityIdProvider : IEntityIdProvider
	{
		public string GetIdFor<T>()
		{
			return "JR2000";
		}
	}
}