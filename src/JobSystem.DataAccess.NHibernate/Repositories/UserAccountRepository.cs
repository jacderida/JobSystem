using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	/// <summary>
	/// The concrete implementation of IUserAccountRepository
	/// </summary>
	public class UserAccountRepository : RepositoryBase<UserAccount>, IUserAccountRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:UserAccountRepository"/> class.
		/// </summary>
		/// <param name="dalConfiguration">Provides access to the database session</param>
		public UserAccountRepository(DalConfiguration dalConfiguration)
			: base(dalConfiguration)
		{
		}

		public UserAccount GetByEmail(string emailAddress, bool readOnly)
		{
			emailAddress = emailAddress.Trim();
			if (string.IsNullOrWhiteSpace(emailAddress))
				throw new ArgumentException("invalid email address");
			var criteria = CurrentSession
				.CreateCriteria<UserAccount>()
				.Add(Restrictions.InsensitiveLike("EmailAddress", emailAddress, MatchMode.Exact));
			if (readOnly)
				criteria.SetFlushMode(FlushMode.Never);
			return criteria.UniqueResult<UserAccount>();
		}

		public IList<UserAccount> GetUsers()
		{
			return CurrentSession.Query<UserAccount>().ToList();
		}
	}
}