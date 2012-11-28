using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IOrderItemRepository : IReadWriteRepository<OrderItem, Guid>
	{
		void CreatePending(PendingOrderItem pendingItem);
		bool JobItemHasPendingOrderItem(Guid jobItemId);
		int GetOrderItemsCount(Guid orderId);
		IEnumerable<OrderItem> GetOrderItems(Guid orderId);
		IEnumerable<OrderItem> GetOrderItemsForJobItem(Guid jobItemId);
		IEnumerable<PendingOrderItem> GetPendingOrderItems();
		IEnumerable<PendingOrderItem> GetPendingOrderItems(IList<Guid> pendingItemIds);
		PendingOrderItem GetPendingOrderItem(Guid id);
		PendingOrderItem GetPendingOrderItemForJobItem(Guid jobItemId);
		void DeletePendingItem(Guid id);
		void UpdatePendingItem(PendingOrderItem pendingItem);
	}
}