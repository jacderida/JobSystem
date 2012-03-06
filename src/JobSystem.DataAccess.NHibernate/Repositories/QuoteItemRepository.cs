using System;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class QuoteItemRepository : RepositoryBase<QuoteItem>, IQuoteItemRepository
	{
		public void CreatePendingQuoteItem(PendingQuoteItem pendingQuoteItem)
		{
			CurrentSession.Save(pendingQuoteItem);
		}

		public PendingQuoteItem GetPendingQuoteItem(Guid id)
		{
			return CurrentSession.Query<PendingQuoteItem>().Where(p => p.Id == id).SingleOrDefault();
		}

		public bool JobItemHasPendingQuoteItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingQuoteItem>().Where(p => p.JobItem.Id == jobItemId).SingleOrDefault() != null;
		}

		public void UpdatePendingItem(PendingQuoteItem pendingItem)
		{
			CurrentSession.Update(pendingItem);
		}

		public IEnumerable<PendingQuoteItem> GetPendingQuoteItems()
		{
			return CurrentSession.Query<PendingQuoteItem>();
		}

		public IEnumerable<PendingQuoteItem> GetPendingQuoteItems(IList<Guid> pendingItemIds)
		{
			var query = Restrictions.Disjunction();
			foreach (var id in pendingItemIds)
				query.Add(Restrictions.Eq("Id", id));
			var criteria = CurrentSession.CreateCriteria<PendingQuoteItem>().Add(query);
			return criteria.List<PendingQuoteItem>();
		}
	}
}