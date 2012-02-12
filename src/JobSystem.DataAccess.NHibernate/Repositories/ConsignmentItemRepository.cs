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
	}
}