using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IDeliveryItemRepository : IReadWriteRepository<DeliveryItem, Guid>
	{
		void CreatePending(PendingDeliveryItem pendingDeliveryItem);
		void DeletePendingDeliveryItem(Guid id);
		bool JobItemHasPendingDeliveryItem(Guid jobItemId);
		IEnumerable<PendingDeliveryItem> GetPendingDeliveryItems();
		IEnumerable<PendingDeliveryItem> GetPendingDeliveryItems(IList<Guid> pendingItemIds);
	}
}