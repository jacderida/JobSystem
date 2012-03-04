using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;
using NHibernate.Criterion;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class ConsignmentItemRepository : RepositoryBase<ConsignmentItem>, IConsignmentItemRepository
	{
		public void CreatePendingItem(PendingConsignmentItem pendingItem)
		{
			CurrentSession.Save(pendingItem);
		}

		public IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems()
		{
			return CurrentSession.Query<PendingConsignmentItem>();
		}

		public IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems(IList<Guid> pendingItemIds)
		{
			var query = Restrictions.Disjunction();
			foreach (var id in pendingItemIds)
				query.Add(Restrictions.Eq("Id", id));
			var criteria = CurrentSession.CreateCriteria<PendingConsignmentItem>().Add(query);
			return criteria.List<PendingConsignmentItem>();
		}

		public PendingConsignmentItem GetPendingConsignmentItem(Guid id)
		{
			return CurrentSession.Query<PendingConsignmentItem>().Where(p => p.Id == id).SingleOrDefault();
		}

		public IEnumerable<ConsignmentItem> GetConsignmentItems(Guid consignmentId)
		{
			return CurrentSession.Query<ConsignmentItem>().Where(ci => ci.Consignment.Id == consignmentId);
		}

		public void DeletePendingItem(Guid id)
		{
			CurrentSession.Delete(CurrentSession.Get<PendingConsignmentItem>(id));
		}

		public bool JobItemHasPendingConsignmentItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingConsignmentItem>().Where(ci => ci.JobItem.Id == jobItemId).SingleOrDefault() != null;
		}

		public PendingConsignmentItem GetPendingItem(Guid id)
		{
			return CurrentSession.Get<PendingConsignmentItem>(id);
		}

		public void UpdatePendingItem(PendingConsignmentItem pendingItem)
		{
			CurrentSession.Update(pendingItem);
		}
	}
}