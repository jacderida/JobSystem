using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IOrderItemRepository : IReadWriteRepository<OrderItem, Guid>
	{
		void CreatePending(PendingOrderItem pendingItem);
		bool JobItemHasPendingOrderItem(Guid jobItemId);
		PendingOrderItem GetPendingOrderItem(Guid id);
		void UpdatePendingItem(PendingOrderItem pendingItem);
	}
}