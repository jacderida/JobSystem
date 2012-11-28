using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class QuoteRepository : RepositoryBase<Quote>, IQuoteRepository
	{
		public int GetQuotesCount()
		{
			return CurrentSession.Query<Quote>().Count();
		}

		public IEnumerable<Quote> GetQuotes()
		{
			return CurrentSession.Query<Quote>();
		}
	}
}