using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using System.Collections.Generic;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class QuoteRepository : RepositoryBase<Quote>, IQuoteRepository
	{
		public IEnumerable<Quote> GetQuotes()
		{
			return CurrentSession.Query<Quote>();
		}
	}
}