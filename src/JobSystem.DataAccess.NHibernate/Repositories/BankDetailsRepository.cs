using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class BankDetailsRepository : RepositoryBase<BankDetails>, IBankDetailsRepository
	{
		public IEnumerable<BankDetails> GetBankDetails()
		{
			return CurrentSession.Query<BankDetails>();
		}
	}
}