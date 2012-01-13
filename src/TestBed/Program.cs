using System;
using System.Configuration;
using System.Data.SqlClient;
using FluentNHibernate.Cfg.Db;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Mappings;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Messaging;
using JobSystem.Queueing.Msmq;
using NHibernate.Connection;

namespace TestBed
{
	public class Program
	{
		static void Main(string[] args)
		{
			//InitialiseNHibernateSessions();
			//var jobItemService = new JobItemService(
			//    new TestUserContext(TestUserContext.CreateAdminUser()), new JobRepository(), new InstrumentRepository(), new ListItemRepository(), new MsmqQueueGateway<IMessage>("", ""), new JobItemRepository());
			//NHibernateSession.Current.BeginTransaction();
			//jobItemService.CreateJobItem(
			//    Guid.Parse("FB51BB05-5510-4BA9-9593-4EF8DD87C3D9"), Guid.NewGuid(), Guid.Parse("338641A0-C524-4CC8-9720-F939DB7C0E18"),
			//    "SER1234", "AS1234", Guid.Parse("5DA8F243-B36C-4B33-9109-0D2A612E2813"), Guid.Parse("ACE917B1-3409-4D14-973B-615ECC3A4EF0"),
			//    Guid.Parse("92864F70-40A1-47B2-BCD0-E832902D50A7"), 12, "instructions", "accessories", false, "return reason", "comments");
			//NHibernateSession.Current.Transaction.Commit();
		}

		private static void InitialiseNHibernateSessions()
		{
			SimpleConnectionProvider.CatalogName = "JobSystem.Development";
			var dbConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(NHibernateSession.GetInitialConnectionString()).Provider<SimpleConnectionProvider>();
			NHibernateSession.Init(
				new SimpleSessionStorage(), new[] { "JobSystem.DataAccess.NHibernate.dll" }, new AutoPersistenceModelGenerator().Generate(), null, null, null, dbConfigurer);
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
				PasswordSalt = "K5fSyL55UIUN1/YyZMIMz/YElG4b1K6oUGsKZn/UC97/wtBP1O9JUJnAcxjergpNEldX3EY4udXdkpJSGpclCw=="
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