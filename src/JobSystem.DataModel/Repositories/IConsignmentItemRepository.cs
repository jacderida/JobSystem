using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IConsignmentItemRepository : IReadWriteRepository<ConsignmentItem, Guid>
	{
		void CreatePendingItem(PendingConsignmentItem pendingItem);
		IEnumerable<ConsignmentItem> GetConsignmentItems(Guid consignmentId);
		IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems();
		IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems(IList<Guid> pendingItemIds);
	}
}