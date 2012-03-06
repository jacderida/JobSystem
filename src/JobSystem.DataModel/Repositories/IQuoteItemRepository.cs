﻿using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IQuoteItemRepository : IReadWriteRepository<QuoteItem, Guid>
	{
		void CreatePendingQuoteItem(PendingQuoteItem pendingQuoteItem);
		PendingQuoteItem GetPendingQuoteItem(Guid id);
		IEnumerable<PendingQuoteItem> GetPendingQuoteItems();
		IEnumerable<PendingQuoteItem> GetPendingQuoteItems(IList<Guid> pendingItemIds);
		bool JobItemHasPendingQuoteItem(Guid jobItemId);
		void UpdatePendingItem(PendingQuoteItem pendingItem);
	}
}