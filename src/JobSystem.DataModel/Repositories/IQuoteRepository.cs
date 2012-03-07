using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IQuoteRepository : IReadWriteRepository<Quote, Guid>
	{
		IEnumerable<Quote> GetQuotes();
	}
}