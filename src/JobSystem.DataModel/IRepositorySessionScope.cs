using System;

namespace JobSystem.DataModel
{
	/// <summary>
	/// Wraps up a specific concrete repository session which can be used to setup transactions to be used by the business logic layer.
	/// </summary>
	public interface IRepositorySessionScope : IDisposable
	{
		/// <summary>
		/// Creates and begins a transaction.
		/// </summary>
		void BeginTransaction();

		/// <summary>
		/// Rollbacks the current transaction.
		/// </summary>
		void RollbackTransaction();

		/// <summary>
		/// Commits the current transaction.
		/// </summary>
		void CommitTransaction();
	}
}