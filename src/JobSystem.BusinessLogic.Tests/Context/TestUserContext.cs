using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.Tests.Context
{
	/// <summary>
	/// A user context for running test cases
	/// </summary>
	public class TestUserContext : IUserContext
	{
		private readonly UserAccount _userAccount;

		public TestUserContext()
			: this(UserAccount.Anonymous)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestUserContext"/> class.
		/// </summary>
		/// <param name="userAccount">The user account to associated with the account</param>
		public TestUserContext(UserAccount userAccount)
		{
			_userAccount = userAccount;
		}

		public TestUserContext(Guid userId, string emailAddress, string userName, string jobTitle)
			: this(new UserAccount()
							{
								Id = userId,
								Name = userName,
								JobTitle = jobTitle,
								EmailAddress = emailAddress
							})
		{
		}

		public static IUserContext Create(string emailAddress, string userName, string jobTitle)
		{
			return new TestUserContext(Guid.Empty, emailAddress, userName, jobTitle);
		}

		#region IUserContext Members

		/// <summary>
		/// Return the currently logged in user. 
		/// </summary>
		/// <returns></returns>
		public UserAccount GetCurrentUser()
		{
			return _userAccount;
		}

		#endregion
	}
}