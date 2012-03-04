using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IQuoteItemRepository : IReadWriteRepository<QuoteItem, Guid>
	{
		void CreatePendingQuoteItem(PendingQuoteItem pendingQuoteItem);
		PendingQuoteItem GetPendingQuoteItem(Guid id);
		bool JobItemHasPendingQuoteItem(Guid jobItemId);
	}
}