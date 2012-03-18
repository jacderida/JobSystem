using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IDeliveryItemRepository : IReadWriteRepository<DeliveryItem, Guid>
	{
		void CreatePending(PendingDeliveryItem pendingDeliveryItem);
		bool JobItemHasPendingDeliveryItem(Guid jobItemId);
	}
}