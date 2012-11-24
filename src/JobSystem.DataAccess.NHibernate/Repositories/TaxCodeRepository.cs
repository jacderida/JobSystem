using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class TaxCodeRepository : RepositoryBase<TaxCode>, ITaxCodeRepository
	{
		public TaxCode GetByName(string name)
		{
			return CurrentSession.Query<TaxCode>().SingleOrDefault(t => t.TaxCodeName == name);
		}

		public IEnumerable<TaxCode> GetTaxCodes()
		{
			return CurrentSession.Query<TaxCode>();
		}
	}
}