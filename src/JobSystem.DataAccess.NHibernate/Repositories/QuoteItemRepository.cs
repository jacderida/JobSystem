using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class QuoteItemRepository : RepositoryBase<QuoteItem>, IQuoteItemRepository
	{
		public void CreatePendingQuoteItem(PendingQuoteItem pendingQuoteItem)
		{
			CurrentSession.Save(pendingQuoteItem);
		}

		public PendingQuoteItem GetPendingQuoteItemForJobItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingQuoteItem>().Where(p => p.JobItem.Id == jobItemId).SingleOrDefault();
		}

		public QuoteItem GetQuoteItemForJobItem(Guid jobItemId)
		{
			return CurrentSession.Query<QuoteItem>().SingleOrDefault(qi => qi.JobItem.Id == jobItemId);
		}

		public IEnumerable<QuoteItem> GetQuoteItems(Guid quoteId)
		{
			return CurrentSession.Query<QuoteItem>().Where(qi => qi.Quote.Id == quoteId);
		}

		public void DeletePendingQuoteItem(Guid id)
		{
			CurrentSession.Delete(CurrentSession.Get<PendingQuoteItem>(id));
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