using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using global::FluentNHibernate.Automapping;
using global::FluentNHibernate.Cfg;
using global::FluentNHibernate.Cfg.Db;
using global::NHibernate;
using global::NHibernate.Cfg;
using JobSystem.Framework.DesignByContract;
using NHibernate.Exceptions;
using JobSystem.DataModel;

namespace JobSystem.DataAccess.NHibernate
{
	public static class NHibernateSession
	{
		/// <summary>
		///     The default factory key used if only one database is being communicated with.
		/// </summary>
		public static readonly string DefaultFactoryKey = "nhibernate.current_session";

		/// <summary>
		///     Maintains a dictionary of NHibernate session factories, one per database.  The key is 
		///     the "factory key" used to look up the associated database, and used to decorate respective
		///     repositories.  If only one database is being used, this dictionary contains a single
		///     factory with a key of <see cref = "DefaultFactoryKey" />.
		/// </summary>
		private static readonly Dictionary<string, ISessionFactory> SessionFactories = new Dictionary<string, ISessionFactory>();
		private static INHibernateConfigurationCache configurationCache;

		/// <summary>
		/// This is just used to build the session factory - the connection string will be obtained using RequestContextConnectionProvider
		/// in the web project.
		/// </summary>
		public static string GetInitialConnectionString()
		{
			var csb = new SqlConnectionStringBuilder();
			csb.DataSource = System.Configuration.ConfigurationManager.AppSettings["DatabaseServer"];
			csb.InitialCatalog = System.Configuration.ConfigurationManager.AppSettings["DatabaseCatalogName"];
			csb.UserID = System.Configuration.ConfigurationManager.AppSettings["DatabaseUsername"];
			csb.Password = System.Configuration.ConfigurationManager.AppSettings["DatabasePassword"];
			return csb.ToString();
		}

		/// <summary>
		///     Used to get the current NHibernate session if you're communicating with a single database.
		///     When communicating with multiple databases, invoke <see cref = "CurrentFor" /> instead.
		/// </summary>
		public static ISession Current
		{
			get
			{
				CheckSharpArch.Require(
					!IsConfiguredForMultipleDatabases(),
					"The NHibernateSession.Current property may " +
					"only be invoked if you only have one NHibernate session factory; i.e., you're " +
					"only communicating with one database.  Since you're configured communications " +
					"with multiple databases, you should instead call CurrentFor(string factoryKey)");
				return CurrentFor(DefaultFactoryKey);
			}
		}

		/*
		 * This is a major hack!
		 * 
		 * It's 
		 * */
		public static void TryCommitTransactionForEntityWithUniqueId()
		{
			try
			{
				if (Current.Transaction.IsActive)
					Current.Transaction.Commit();
			}
			catch (StaleObjectStateException ex)
			{
				Current.Transaction.Rollback();
				Current.Clear();
				throw new EntityIdNotUniqueException(ex.Message);
			}
		}

		/// <summary>
		///     An application-specific implementation of ISessionStorage must be setup either thru
		///     <see cref = "InitStorage" /> or one of the <see cref = "Init" /> overloads.
		/// </summary>
		public static ISessionStorage Storage { get; set; }

		[CLSCompliant(false)]
		public static Configuration AddConfiguration(
			string factoryKey,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			IDictionary<string, string> cfgProperties,
			string validatorCfgFile,
			IPersistenceConfigurer persistenceConfigurer)
		{
			Configuration config;
			config = AddConfiguration(
				factoryKey, mappingAssemblies, autoPersistenceModel, ConfigureNHibernate(cfgFile, cfgProperties), validatorCfgFile, persistenceConfigurer);
			return config;
		}

		[CLSCompliant(false)]
		public static Configuration AddConfiguration(
			string factoryKey,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			Configuration cfg,
			string validatorCfgFile,
			IPersistenceConfigurer persistenceConfigurer)
		{
			var sessionFactory = CreateSessionFactoryFor(mappingAssemblies, autoPersistenceModel, cfg, persistenceConfigurer);
			return AddConfiguration(factoryKey, sessionFactory, cfg, validatorCfgFile);
		}

		[CLSCompliant(false)]
		public static Configuration AddConfiguration(
			string factoryKey, ISessionFactory sessionFactory, Configuration cfg, string validatorCfgFile)
		{
			CheckSharpArch.Require(
				!SessionFactories.ContainsKey(factoryKey),
				"A session factory has already been configured with the key of " + factoryKey);
			SessionFactories.Add(factoryKey, sessionFactory);
			return cfg;
		}

		/// <summary>
		///     This method is used by application-specific session storage implementations
		///     and unit tests. Its job is to walk thru existing cached sessions and Close() each one.
		/// </summary>
		public static void CloseAllSessions()
		{
			if (Storage != null)
			{
				foreach (var session in Storage.GetAllSessions())
				{
					if (session.IsOpen)
						session.Close();
				}
			}
		}

		/// <summary>
		///     Used to get the current NHibernate session associated with a factory key; i.e., the key 
		///     associated with an NHibernate session factory for a specific database.
		/// 
		///     If you're only communicating with one database, you should call <see cref = "Current" /> instead,
		///     although you're certainly welcome to call this if you have the factory key available.
		/// </summary>
		public static ISession CurrentFor(string factoryKey)
		{
			CheckSharpArch.Require(!string.IsNullOrEmpty(factoryKey), "factoryKey may not be null or empty");
			CheckSharpArch.Require(Storage != null, "An ISessionStorage has not been configured");
			CheckSharpArch.Require(
				SessionFactories.ContainsKey(factoryKey),
				"An ISessionFactory does not exist with a factory key of " + factoryKey);
			var session = Storage.GetSessionForKey(factoryKey);
			if (session == null)
			{
				session = SessionFactories[factoryKey].OpenSession();
				Storage.SetSessionForKey(factoryKey, session);
			}
			return session;
		}

		/// <summary>
		///     Returns the default ISessionFactory using the DefaultFactoryKey.
		/// </summary>
		public static ISessionFactory GetDefaultSessionFactory()
		{
			return GetSessionFactoryFor(DefaultFactoryKey);
		}

		/// <summary>
		///     Return an ISessionFactory based on the specified factoryKey.
		/// </summary>
		public static ISessionFactory GetSessionFactoryFor(string factoryKey)
		{
			if (!SessionFactories.ContainsKey(factoryKey))
				return null;
			return SessionFactories[factoryKey];
		}

		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies)
		{
			return Init(storage, mappingAssemblies, null, null, null, null, null);
		}

		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile)
		{
			return Init(storage, mappingAssemblies, null, cfgFile, null, null, null);
		}

		public static Configuration Init(
			ISessionStorage storage, string[] mappingAssemblies, IDictionary<string, string> cfgProperties)
		{
			return Init(storage, mappingAssemblies, null, null, cfgProperties, null, null);
		}

		public static Configuration Init(
			ISessionStorage storage, string[] mappingAssemblies, string cfgFile, string validatorCfgFile)
		{
			return Init(storage, mappingAssemblies, null, cfgFile, null, validatorCfgFile, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, null, null, null, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, null, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			IDictionary<string, string> cfgProperties)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, null, cfgProperties, null, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			string validatorCfgFile)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, validatorCfgFile, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			IDictionary<string, string> cfgProperties,
			string validatorCfgFile)
		{
			return Init(
				storage, mappingAssemblies, autoPersistenceModel, cfgFile, cfgProperties, validatorCfgFile, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			IDictionary<string, string> cfgProperties,
			string validatorCfgFile,
			IPersistenceConfigurer persistenceConfigurer)
		{
			InitStorage(storage);
			try
			{
				return AddConfiguration(
					DefaultFactoryKey, mappingAssemblies, autoPersistenceModel, cfgFile, cfgProperties, validatorCfgFile, persistenceConfigurer);
			}
			catch
			{
				// If this NHibernate config throws an exception, null the Storage reference so 
				// the config can be corrected without having to restart the web application.
				Storage = null;
				throw;
			}
		}

		public static void InitStorage(ISessionStorage storage)
		{
			CheckSharpArch.Require(storage != null, "storage mechanism was null but must be provided");
			CheckSharpArch.Require(Storage == null, "A storage mechanism has already been configured for this application");
			Storage = storage;
		}

		public static bool IsConfiguredForMultipleDatabases()
		{
			return SessionFactories.Count > 1;
		}

		public static void RemoveSessionFactoryFor(string factoryKey)
		{
			if (GetSessionFactoryFor(factoryKey) != null)
				SessionFactories.Remove(factoryKey);
		}

		/// <summary>
		///     To facilitate unit testing, this method will reset this object back to its
		///     original state before it was configured.
		/// </summary>
		public static void Reset()
		{
			if (Storage != null)
				foreach (var session in Storage.GetAllSessions())
					session.Dispose();
			SessionFactories.Clear();
			Storage = null;
		}

		private static Configuration ConfigureNHibernate(string cfgFile, IDictionary<string, string> cfgProperties)
		{
			var cfg = new Configuration();
			if (cfgProperties != null)
				cfg.AddProperties(cfgProperties);
			if (!String.IsNullOrEmpty(cfgFile))
				return cfg.Configure(cfgFile);
			if (File.Exists("Hibernate.cfg.xml"))
				return cfg.Configure();
			return cfg;
		}

		private static ISessionFactory CreateSessionFactoryFor(
			IEnumerable<string> mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			Configuration cfg,
			IPersistenceConfigurer persistenceConfigurer)
		{
			var fluentConfiguration = Fluently.Configure(cfg);
			if (persistenceConfigurer != null)
				fluentConfiguration.Database(persistenceConfigurer);
			fluentConfiguration.Mappings(m =>
			{
				foreach (var mappingAssembly in mappingAssemblies)
				{
					var assembly = Assembly.LoadFrom(MakeLoadReadyAssemblyName(mappingAssembly));
					m.HbmMappings.AddFromAssembly(assembly);
					m.FluentMappings.AddFromAssembly(assembly).Conventions.AddAssembly(assembly);
				}
				if (autoPersistenceModel != null)
					m.AutoMappings.Add(autoPersistenceModel);
			});
			return fluentConfiguration.BuildSessionFactory();
		}

		private static string MakeLoadReadyAssemblyName(string assemblyName)
		{
			return (assemblyName.IndexOf(".dll") == -1) ? assemblyName.Trim() + ".dll" : assemblyName.Trim();
		}
	}
}