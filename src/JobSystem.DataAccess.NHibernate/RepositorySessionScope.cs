using System;
using JobSystem.DataModel;
using NHibernate;

namespace JobSystem.DataAccess.NHibernate
{
	/// <summary>
	/// Creates a new NHibernate respository session scope
	/// </summary>
	public class RepositorySessionScope : IRepositorySessionScope
	{
		/// <summary>
		/// Provides access to the database session
		/// </summary>
		private readonly DalConfiguration _dalConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="RepositorySessionScope"/> class, opening a new NHibernate session.
		/// </summary>
		/// <param name="dalConfiguration">Provides access to the database session</param>
		public RepositorySessionScope(DalConfiguration dalConfiguration)
		{
			_dalConfiguration = dalConfiguration;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// In this case the NHibernate Session and any associated transaction
		/// </summary>
		public void Dispose()
		{
			//DalConfiguration.GetCurrentSession().Dispose();
		}

		/// <summary>
		/// Creates and begins an NHibernate transaction.
		/// </summary>
		public void BeginTransaction()
		{
			if (InProgress())
				GetTransaction().Dispose();
			_dalConfiguration.GetCurrentSession().BeginTransaction();
		}

		/// <summary>
		/// Rollbacks the current NHibernate transaction.
		/// </summary>
		public void RollbackTransaction()
		{
			var transaction = GetTransaction();
			if (transaction.IsActive)
			{
				transaction.Rollback();
				_dalConfiguration.CloseCurrentSession();
			}
		}

		/// <summary>
		/// Commits the current NHibernate transaction.
		/// </summary>
		public void CommitTransaction()
		{
			var transaction = GetTransaction();
			if (!transaction.IsActive)
				throw new InvalidOperationException("Must call Start() on the unit of work before committing");
			transaction.Commit();
			//Flush();
		}

		/// <summary>
		/// Flush the nhibernate session
		/// </summary>
		public void Flush()
		{
			var session = _dalConfiguration.GetCurrentSession();
			session.Flush();
			session.Close();
		}

		protected bool InProgress()
		{
			var transaction = GetTransaction();
			return transaction.IsActive || transaction.WasCommitted || transaction.WasRolledBack;
		}

		protected ITransaction GetTransaction()
		{
			return _dalConfiguration.GetCurrentSession().Transaction;
		}
	}
}