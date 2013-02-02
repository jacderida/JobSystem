using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public IEnumerable<Currency> GetCurrencies()
        {
            return CurrentSession.Query<Currency>();
        }
    }
}