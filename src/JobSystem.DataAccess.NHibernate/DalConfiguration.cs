// <copyright company="Gael Limited">
// Copyright (c) 2010 All Right Reserved
// </copyright>
// <author></author>
// <email></email>
// <date>2010</date>
// <summary>
//	Complying with all copyright laws is the responsibility of the
//	user. Without limiting rights under copyrights, neither the
//	whole nor any part of this document may be reproduced, stored
//	in or introduced into a retrieval system, or transmitted in any
//	form or by any means (electronic, mechanical, photocopying,
//	recording, or otherwise), or for any purpose without express
//	written permission of Gael Limited.
// </summary>

using System.Configuration;
using System.Data.SqlClient;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using global::NHibernate.Tool.hbm2ddl;
using JobSystem.DataAccess.NHibernate.Mappings.Conventions;
using nh = global::NHibernate;

namespace JobSystem.DataAccess.NHibernate
{
	/// <summary>
	/// A utility class for configuring NHibernate databases and sessions.
	/// </summary>
	public class DalConfiguration
	{
		#region Fields

		private const string NHibernateSessionKey = "NHibernate.ISession";
		private readonly ILocalStorage<nh.ISession> _sessionStorage = new LocalStorage<nh.ISession>();
		private global::NHibernate.Cfg.Configuration _dbConfiguration;
		private nh.ISessionFactory _sessionFactory;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:DalConfiguration"/> class.
		/// </summary>
		protected DalConfiguration()
		{
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Creates and returns a new <see cref="DalConfiguration"/> instance
		/// </summary>
		/// <typeparam name="T_ConnProvider">typeof the connection provider</typeparam>
		/// <returns>new <see cref="DalConfiguration"/> instance</returns>
		public static DalConfiguration CreateInstance<T_ConnProvider>()
			where T_ConnProvider : global::NHibernate.Connection.IConnectionProvider
		{
			var dal = new DalConfiguration();
			dal.ConfigureAndCacheSessionFactory<T_ConnProvider>();
			return dal;
		}

		/// <summary>
		/// Close Current Session
		/// </summary>
		public void CloseCurrentSession()
		{
			nh.ISession session = _sessionStorage[NHibernateSessionKey];
			if (session != null && session.IsOpen)
				session.Close();
			_sessionStorage[NHibernateSessionKey] = null;
		}

		/// <summary>
		/// Creates a database using the Fluent NHibernate auto mapper, with a schema based on the entities defined in the Alea.DataModel.Entities namespace.
		/// The database to connect to should have its details specified under the appSettings section in the application config file,
		/// using the keys "DatabaseServerName", "DatabaseCatalogName", "DatabaseUsername", and "DatabasePassword".
		/// </summary>
		public void CreateDatabase()
		{
			var schemaExport = new SchemaExport(_dbConfiguration);
			schemaExport.Drop(false, true);
			schemaExport.Create(true, true);
		}

		/// <summary>
		/// Creates a database using the Fluent NHibernate auto mapper, with a schema based on the entities defined in the Alea.DataModel.Entities namespace.
		/// The database to connect to should have its details specified under the appSettings section in the application config file,
		/// using the keys "DatabaseServerName", "DatabaseCatalogName", "DatabaseUsername", and "DatabasePassword".
		/// The sql used to create the schema will be output to the location specified.
		/// </summary>
		/// <param name="scriptFilePath">The location to output the sql script to.</param>
		public void CreateDatabaseWithScriptOutput(string scriptFilePath)
		{
			var schemaExport = new SchemaExport(_dbConfiguration);
			schemaExport.Drop(false, true);
			schemaExport.SetOutputFile(scriptFilePath);
			schemaExport.Create(true, true);
		}

		/// <summary>
		/// Opens a new session when one is currently used, otherwise returns the session currently in use.
		/// </summary>
		/// <returns>The Nhibernate Sesssion</returns>
		public nh.ISession GetCurrentSession()
		{
			nh.ISession session = _sessionStorage[NHibernateSessionKey];
			if (session == null || !session.IsOpen)
			{
				session = OpenSession();
				_sessionStorage[NHibernateSessionKey] = session;
			}
			return session;
		}

		/// <summary>
		/// Updates a database using the Fluent NHibernate auto mapper, with a schema based on the entities defined in the Alea.DataModel.Entities namespace.
		/// Any new database schema will be appended to the existing schema without overwriting it or any existing data.
		/// The database to connect to to update should have its details specified under the appSettings section in the application config file,
		/// using the keys "DatabaseServerName", "DatabaseCatalogName", "DatabaseUsername", and "DatabasePassword".
		/// </summary>
		public void UpdateDatabase()
		{
			var schemaUpdate = new nh.Tool.hbm2ddl.SchemaUpdate(_dbConfiguration);
			schemaUpdate.Execute(true, true);
		}

		private static string GetConnectionString()
		{
			var csb = new SqlConnectionStringBuilder();
			csb.DataSource = ConfigurationManager.AppSettings["DatabaseServer"];
			csb.InitialCatalog = ConfigurationManager.AppSettings["DatabaseCatalogName"];
			csb.UserID = ConfigurationManager.AppSettings["DatabaseUsername"];
			csb.Password = ConfigurationManager.AppSettings["DatabasePassword"];
			return csb.ToString();
		}

		/// <summary>
		/// Creates, configures and caches the session factory instance used to create database sessions.
		/// This process is costly, so should ideally occur only once for the application.
		/// </summary>
		/// <typeparam name="T_ConnProvider">Typeof instance providing the connection string for the database connection</typeparam>
		private void ConfigureAndCacheSessionFactory<T_ConnProvider>()
			where T_ConnProvider : global::NHibernate.Connection.IConnectionProvider
		{
			var dbConfigurer =
				MsSqlConfiguration.MsSql2008
				.ConnectionString(GetConnectionString())
				.Provider<T_ConnProvider>();
			var autoPersistenceModel = new AutoPersistenceModelGenerator().Generate();
				 _sessionFactory =
				Fluently.Configure()
				.Database(dbConfigurer)
				.Mappings(m => m.AutoMappings.Add(autoPersistenceModel))
				.ExposeConfiguration(c => _dbConfiguration = c)
				.BuildSessionFactory();
		}

		/// <summary>
		/// Gets a session from the session factory. If a session factory doesn't exist on access, one will be created.
		/// </summary>
		/// <returns>A session to use to commit or return objects from the database.</returns>
		private nh.ISession OpenSession()
		{
			var session = _sessionFactory.OpenSession();
			session.FlushMode = global::NHibernate.FlushMode.Commit;
			return session;
		}

		#endregion Methods
	}
}