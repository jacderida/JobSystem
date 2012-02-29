using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IQuoteItemRepository : IReadWriteRepository<QuoteItem, Guid>
	{
	}
}