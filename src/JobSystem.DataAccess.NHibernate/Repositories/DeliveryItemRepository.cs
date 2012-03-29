using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class DeliveryItemRepository : RepositoryBase<DeliveryItem>, IDeliveryItemRepository
	{
		public void CreatePending(PendingDeliveryItem pendingDeliveryItem)
		{
			CurrentSession.Save(pendingDeliveryItem);
		}

		public void DeletePendingDeliveryItem(Guid id)
		{
			CurrentSession.Delete(CurrentSession.Get<PendingDeliveryItem>(id));
		}

		public bool JobItemHasPendingDeliveryItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingDeliveryItem>().SingleOrDefault(di => di.JobItem.Id == jobItemId) != null;
		}

		public DeliveryItem GetDeliveryItemForJobItem(Guid jobItemId)
		{
			return CurrentSession.Query<DeliveryItem>().Where(di => di.JobItem.Id == jobItemId).SingleOrDefault();
		}

		public PendingDeliveryItem GetPendingDeliveryItemForJobItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingDeliveryItem>().Where(p => p.JobItem.Id == jobItemId).SingleOrDefault();
		}

		public PendingDeliveryItem GetPendingDeliveryItem(Guid pendingItemId)
		{
			return CurrentSession.Query<PendingDeliveryItem>().SingleOrDefault();
		}

		public void UpdatePendingDeliveryItem(PendingDeliveryItem pendingItem)
		{
			CurrentSession.Update(pendingItem);
		}

		public IEnumerable<PendingDeliveryItem> GetPendingDeliveryItems()
		{
			return CurrentSession.Query<PendingDeliveryItem>();
		}

		public IEnumerable<PendingDeliveryItem> GetPendingDeliveryItems(IList<Guid> pendingItemIds)
		{
			var query = Restrictions.Disjunction();
			foreach (var id in pendingItemIds)
				query.Add(Restrictions.Eq("Id", id));
			var criteria = CurrentSession.CreateCriteria<PendingDeliveryItem>().Add(query);
			return criteria.List<PendingDeliveryItem>();
		}

		public IEnumerable<DeliveryItem> GetDeliveryItems(Guid deliveryId)
		{
			return CurrentSession.Query<DeliveryItem>().Where(di => di.Delivery.Id == deliveryId);
		}
	}
}