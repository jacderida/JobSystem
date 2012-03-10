using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IOrderItemRepository : IReadWriteRepository<OrderItem, Guid>
	{
		void CreatePending(PendingOrderItem pendingItem);
		bool JobItemHasPendingOrderItem(Guid jobItemId);
	}
}