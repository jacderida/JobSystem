using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Mappings;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;
using JobSystem.Framework.Security;
using NHibernate.Linq;

namespace JobSystem.Admin.DbWireup
{
	public class JobSystemDatabaseCreationService : IJobSystemDatabaseCreationService
	{
		private const string DatabaseExistsQuery = "USE master; SELECT COUNT(*) FROM sysdatabases WHERE name='{0}'";
		private const string LoginExistsQuery = "USE master; SELECT COUNT(*) from master..syslogins WHERE name = '{0}'";
		private const string CreateLoginQuery = "CREATE LOGIN [{0}] WITH PASSWORD=N'{1}', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF";
		private const string ModifyLoginRoleQuery = "EXEC master..sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'";
		private const string CreateUserQuery = "USE [{0}]; CREATE USER [{1}] FOR LOGIN [{2}];";
		private readonly string _databaseName;
		private string _loginPassword;

		/// <summary>
		/// Initialises an instance of the JobSystemDatabaseCreationService class.
		/// </summary>
		/// <param name="databaseName">The name of the database to create, which will be prefixed with "jobsystem."</param>
		public JobSystemDatabaseCreationService(string databaseName)
		{
			_databaseName = String.Format("jobsystem.{0}", databaseName).ToLower();
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

		public void CreateServerLogin()
		{
			var conn = GetConnection();
			try
			{
				conn.Open();
				var cmd = conn.CreateCommand();
				if (!LoginExists(conn, _databaseName))
				{
					_loginPassword = Guid.NewGuid().ToString();
					cmd.CommandText = String.Format(CreateLoginQuery, _databaseName, _loginPassword);
					cmd.ExecuteNonQuery();
					cmd.CommandText = String.Format(ModifyLoginRoleQuery, _databaseName);
					cmd.ExecuteNonQuery();
				}
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

		public void CreateUserLogin()
		{
			var conn = GetConnection();
			try
			{
				conn.Open();
				ExecuteCreateUserLoginCommand(conn);
				ExecuteAddRoleCommands(conn);
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

		public string GetGeneratedPassword()
		{
			return String.Format("Generated password for {0}: {1}", _databaseName, _loginPassword);
		}

		public void CreateJobSystemSchemaFromMigrations(string migrationsAssemblyPath)
		{
			CreateJobSystemSchemaFromMigrations(migrationsAssemblyPath, GetConnectionStringForCatalog(_databaseName), "sqlserver");
		}

		public void CreateJobSystemSchemaFromMigrations(string migrationsAssemblyPath, string connectionString, string providerType)
		{
			MigrationsSchemaBuilder.CreateSchemaFromMigrations(migrationsAssemblyPath, connectionString, providerType);
		}

		public void InitHibernate()
		{
			SimpleConnectionProvider.CatalogName = _databaseName;
			var dbConfigurer = MsSqlConfiguration.MsSql2008.ConnectionString(NHibernateSession.GetInitialConnectionString()).Provider<SimpleConnectionProvider>();
			InitHibernate(dbConfigurer);
		}

		public void InitHibernate(IPersistenceConfigurer dbConfigurer)
		{
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
			foreach (var listItemCategory in defaultData.ListItemCategories)
				session.Save(listItemCategory, listItemCategory.Id);
			foreach (var type in defaultData.JobTypes)
			{
				var listItem = type.Item2;
				listItem.Category = session.Get<ListItemCategory>(type.Item1);
				session.Save(listItem);
			}
			foreach (var type in defaultData.CertificateTypes)
			{
				var listItem = type.Item2;
				listItem.Category = session.Get<ListItemCategory>(type.Item1);
				session.Save(listItem);
			}
			foreach (var location in defaultData.JobItemLocations)
			{
				var listItem = location.Item2;
				listItem.Category = session.Get<ListItemCategory>(location.Item1);
				session.Save(listItem);
			}
			foreach (var location in defaultData.JobItemInitialLocations)
			{
				var listItem = location.Item2;
				listItem.Category = session.Get<ListItemCategory>(location.Item1);
				session.Save(listItem);
			}
			foreach (var statusItem in defaultData.JobItemWorkStatusItems)
			{
				var listItem = statusItem.Item2;
				listItem.Category = session.Get<ListItemCategory>(statusItem.Item1);
				session.Save(listItem);
			}
			foreach (var statusItem in defaultData.JobItemInitialStatusItems)
			{
				var listItem = statusItem.Item2;
				listItem.Category = session.Get<ListItemCategory>(statusItem.Item1);
				session.Save(listItem);
			}
			foreach (var statusItem in defaultData.JobItemStatusItems)
			{
				var listItem = statusItem.Item2;
				listItem.Category = session.Get<ListItemCategory>(statusItem.Item1);
				session.Save(listItem);
			}
			foreach (var workType in defaultData.JobItemWorkTypes)
			{
				var listItem = workType.Item2;
				listItem.Category = session.Get<ListItemCategory>(workType.Item1);
				session.Save(listItem);
			}
			foreach (var category in defaultData.JobItemCategories)
			{
				var listItem = category.Item2;
				listItem.Category = session.Get<ListItemCategory>(category.Item1);
				session.Save(listItem);
			}
			foreach (var paymentTerm in defaultData.PaymentTerms)
			{
				var listItem = paymentTerm.Item2;
				listItem.Category = session.Get<ListItemCategory>(paymentTerm.Item1);
				session.Save(listItem);
			}
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
				Roles = UserRole.Admin | UserRole.JobApprover | UserRole.OrderApprover | UserRole.Member | UserRole.Manager,
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

		public Currency GetCurrency(Guid id)
		{
			return NHibernateSession.Current.Get<Currency>(id);
		}

		public ListItem GetPaymentTerm(Guid id)
		{
			return NHibernateSession.Current.Get<ListItem>(id);
		}

		private SqlConnection GetConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["JobSystem"].ConnectionString);
		}

		private string GetConnectionStringForCatalog(string databaseName)
		{
			var csb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["JobSystem"].ConnectionString);
			csb.InitialCatalog = databaseName;
			return csb.ToString();
		}

		private void ExecuteCreateUserLoginCommand(SqlConnection conn)
		{
			var cmd = conn.CreateCommand();
			if (!LoginExists(conn, _databaseName))
				throw new InvalidOperationException(
					String.Format("Cannot create a user for login {0}, because the login doesn't exist.", _databaseName));
			cmd.CommandText = String.Format(CreateUserQuery, _databaseName, _databaseName, _databaseName);
			cmd.ExecuteNonQuery();
		}

		private void ExecuteAddRoleCommands(SqlConnection conn)
		{
			var cmd = GetAddUserRoleCommand(conn, "db_datawriter");
			cmd.ExecuteNonQuery();
			cmd = GetAddUserRoleCommand(conn, "db_datareader");
			cmd.ExecuteNonQuery();
		}

		private SqlCommand GetAddUserRoleCommand(SqlConnection conn, string role)
		{
			var cmd = new SqlCommand("sp_addrolemember", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@rolename", role));
			cmd.Parameters.Add(new SqlParameter("@membername", _databaseName));
			return cmd;
		}

		private bool DatabaseExists(SqlConnection conn, string databaseName)
		{
			var cmd = new SqlCommand(String.Format(DatabaseExistsQuery, databaseName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}

		private bool LoginExists(SqlConnection conn, string loginName)
		{
			var cmd = new SqlCommand(String.Format(LoginExistsQuery, loginName), conn);
			return (int)cmd.ExecuteScalar() == 1;
		}
	}
}