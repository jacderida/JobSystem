using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class TaxCodeRepository : RepositoryBase<TaxCode>, ITaxCodeRepository
	{
		public IEnumerable<TaxCode> GetTaxCodes()
		{
			return CurrentSession.Query<TaxCode>();
		}
	}
}