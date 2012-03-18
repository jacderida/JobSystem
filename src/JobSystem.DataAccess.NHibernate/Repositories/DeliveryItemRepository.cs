using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;
using System;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class DeliveryItemRepository : RepositoryBase<DeliveryItem>, IDeliveryItemRepository
	{
		public void CreatePending(PendingDeliveryItem pendingDeliveryItem)
		{
			CurrentSession.Save(pendingDeliveryItem);
		}

		public bool JobItemHasPendingDeliveryItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingDeliveryItem>().SingleOrDefault(di => di.JobItem.Id == jobItemId) != null;
		}
	}
}