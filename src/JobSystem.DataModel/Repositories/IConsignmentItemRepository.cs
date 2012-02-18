using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IConsignmentItemRepository : IReadWriteRepository<ConsignmentItem, Guid>
	{
		void CreatePendingItem(PendingConsignmentItem pendingItem);
		void DeletePendingItem(Guid id);
		PendingConsignmentItem GetPendingItem(Guid id);
		void UpdatePendingItem(PendingConsignmentItem pendingItem);
		bool JobItemHasPendingConsignmentItem(Guid jobItemId);
		IEnumerable<ConsignmentItem> GetConsignmentItems(Guid consignmentId);
		IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems();
		IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems(IList<Guid> pendingItemIds);
	}
}