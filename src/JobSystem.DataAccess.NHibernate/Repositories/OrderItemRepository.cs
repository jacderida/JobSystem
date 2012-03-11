using System;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
	{
		public void CreatePending(PendingOrderItem pendingItem)
		{
			CurrentSession.Save(pendingItem);
		}

		public bool JobItemHasPendingOrderItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingOrderItem>().SingleOrDefault(p => p.JobItem.Id == jobItemId) != null;
		}

		public PendingOrderItem GetPendingOrderItem(Guid id)
		{
			return CurrentSession.Query<PendingOrderItem>().SingleOrDefault(p => p.Id == id);
		}

		public void UpdatePendingItem(PendingOrderItem pendingItem)
		{
			CurrentSession.Save(pendingItem);
		}
	}
}