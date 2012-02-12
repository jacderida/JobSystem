using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class ConsignmentItemRepository : RepositoryBase<ConsignmentItem>, IConsignmentItemRepository
	{
		public void CreatePendingItem(PendingConsignmentItem pendingItem)
		{
			CurrentSession.Save(pendingItem);
		}

		public IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems()
		{
			return null;
		}

		public IEnumerable<PendingConsignmentItem> GetPendingConsignmentItems(IList<Guid> pendingItemIds)
		{
			return null;
		}
	}
}