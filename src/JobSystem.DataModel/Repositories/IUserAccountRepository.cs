using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	/// <summary>
	/// A database repository interface for performing tasks related to user accounts.
	/// </summary>
	public interface IUserAccountRepository : IReadWriteRepository<UserAccount, Guid>
	{
		/// <summary>
		/// Gets a list of all the users in the system.
		/// </summary>
		/// <returns>A list of all the users in the system.</returns>
		IList<UserAccount> GetUsers();

		/// <summary>
		/// Get a user account via the email address
		/// </summary>
		/// <param name="emailAddress">The email address of the user</param>
		/// <param name="readOnly">True if you plan to the make changes to the entity</param>
		/// <returns>The AccountUser associated with the email address if one exists, otherwise null</returns>
		UserAccount GetByEmail(string emailAddress, bool readOnly);
	}
}