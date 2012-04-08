using System;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
	{
		public void CreatePendingItem(PendingInvoiceItem pendingItem)
		{
			CurrentSession.Save(pendingItem);
		}

		public bool JobItemHasPendingInvoiceItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingInvoiceItem>().Where(j => j.JobItem.Id == jobItemId).SingleOrDefault() != null;
		}
	}
}