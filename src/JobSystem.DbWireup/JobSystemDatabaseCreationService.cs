using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Mappings;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Security;
using NHibernate.Linq;
using JobSystem.Framework;

namespace JobSystem.DbWireup
{
	public class JobSystemDatabaseCreationService : IJobSystemDatabaseCreationService
	{
		private const string DatabaseExistsQuery = "USE master; SELECT COUNT(*) FROM sysdatabases WHERE name='{0}'";
		private readonly string _databaseName;
		private readonly string _connectionName;


		/// <summary>
		/// Initialises an instance of the JobSystemDatabaseCreationService class.
		/// </summary>
		/// <param name="connectionName">The name of the initial connection (which should be configured to point to the master database).</param>
		/// <param name="databaseName">The name of the database to create, which will be prefixed with "JobSystem."</param>
		public JobSystemDatabaseCreationService(string connectionName, string databaseName)
		{
			_connectionName = connectionName;
			_databaseName = String.Format("JobSystem.{0}", databaseName);
		}

		public void CreateDatabase(bool dropExisting = false)
		{
			var conn = GetConnection();
			try
			{
				conn.Open();
				var cmd = conn.CreateCommand();
				if (DatabaseExists(conn, _databaseName))
				{
					if (!dropExisting)
						throw new InvalidOperationException(String.Format("A database named {0} already exists"));
					cmd.CommandText = String.Format("DROP DATABASE [{0}]", _databaseName);
					cmd.ExecuteNonQuery();
				}
				cmd.CommandText = String.Format("CREATE DATABASE [{0}]", _databaseName);
				cmd.ExecuteNonQuery();
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				conn.Close();
			}
		}

		public void CreateJobSystemSchemaFromMigrations(string migrationsAssemblyPath)
		{
			MigrationsSchemaBuilder.CreateSchemaFromMigrations(migrationsAssemblyPath, GetConnectionStringForCatalog(_databaseName), "sqlserver");
		}

		public void InitHibernate()
		{
			SimpleConnectionProvider.CatalogName = _databaseName;
			var dbConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(NHibernateSession.GetInitialConnectionString()).Provider<SimpleConnectionProvider>();
			NHibernateSession.Init(
				new SimpleSessionStorage(), new[] { "JobSystem.DataAccess.NHibernate.dll" }, new AutoPersistenceModelGenerator().Generate(), null, null, null, dbConfigurer);
		}

		public void BeginHibernateTransaction()
		{
			NHibernateSession.Current.BeginTransaction();
		}

		public void CommitHibernateTransaction()
		{
			NHibernateSession.Current.Transaction.Commit();
		}

		public void RollbackHibernateTransaction()
		{
			NHibernateSession.Current.Transaction.Rollback();
		}

		public void InsertDefaultData(JobSystemDefaultData defaultData)
		{
			if (!NHibernateSession.Current.Transaction.IsActive)
				throw new InvalidOperationException("A transaction must be in progress before default data can be inserted");
			var session = NHibernateSession.Current;
			foreach (var type in defaultData.JobTypes)
				session.Save(type);
			foreach (var location in defaultData.JobItemLocations)
				session.Save(location);
			foreach (var statusItem in defaultData.JobItemWorkStatusItems)
				session.Save(statusItem);
			foreach (var statusItem in defaultData.JobItemInitialStatusItems)
				session.Save(statusItem);
			foreach (var statusItem in defaultData.JobItemStatusItems)
				session.Save(statusItem);
			foreach (var workType in defaultData.JobItemWorkTypes)
				session.Save(workType);
			foreach (var field in defaultData.JobItemFields)
				session.Save(field);
			foreach (var paymentTerm in defaultData.PaymentTerms)
				session.Save(paymentTerm);
			foreach (var taxCode in defaultData.TaxCodes)
				session.Save(taxCode);
			foreach (var currency in defaultData.Currencies)
				session.Save(currency);
			foreach (var bankDetails in defaultData.BankDetails)
				session.Save(bankDetails);
			foreach (var entityIdLookup in defaultData.EntityIdLookups)
				session.Save(entityIdLookup);
		}

		public void InsertCompanyDetails(CompanyDetails companyDetails)
		{
			if (!NHibernateSession.Current.Transaction.IsActive)
				throw new InvalidOperationException("A transaction must be in progress before default data can be inserted");
			var session = NHibernateSession.Current;
			var count = session.Query<CompanyDetails>().Count();
			if (count > 0)
				throw new InvalidOperationException("There is already a company associated with this database");
			session.Save(companyDetails);
		}

		public void InsertAdminUser(string emailAddress, string name, string jobTitle, string password)
		{
			if (!NHibernateSession.Current.Transaction.IsActive)
				throw new InvalidOperationException("A transaction must be in progress before default data can be inserted");
			var cryptographyService = new CryptographicService();
			var passwordSalt = cryptographyService.GenerateSalt();
			var adminUser = new UserAccount
			{
				Id = Guid.NewGuid(),
				Name = name,
				EmailAddress = emailAddress,
				PasswordSalt = passwordSalt,
				PasswordHash = cryptographyService.ComputeHash(password, passwordSalt),
				Roles = UserRole.Admin | UserRole.JobApprover | UserRole.OrderApprover | UserRole.Member,
				JobTitle = jobTitle,
			};
			NHibernateSession.Current.Save(adminUser);
		}

		public BankDetails GetBankDetails(Guid id)
		{
			return NHibernateSession.Current.Get<BankDetails>(id);
		}

		public TaxCode GetTaxCode(Guid id)
		{
			return NHibernateSession.Current.Get<TaxCode>(id);
		}

		public ListItem GetCurrency(Guid id)
		{
			return NHibernateSession.Current.Get<ListItem>(id);
		}

		public ListItem GetPaymentTerm(Guid id)
		{
			return NHibernateSession.Current.Get<ListItem>(id);
		}

		private SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString);
		}

		private string GetConnectionStringForCatalog(string databaseName)
		{
			var csb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString);
			csb.InitialCatalog = databaseName;
			return csb.ToString();
		}

		private bool DatabaseExists(SqlConnection conn, string databaseName)
		{
			if (conn.State != ConnectionState.Open)
				throw new InvalidOperationException("The database connection must be open to query for an existing database");
			var cmd = new SqlCommand(String.Format(DatabaseExistsQuery, databaseName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}
	}
}