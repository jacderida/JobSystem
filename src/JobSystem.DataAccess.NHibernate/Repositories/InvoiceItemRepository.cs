using System;
using System.Collections.Generic;
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

		public void DeletePendingItem(Guid id)
		{
			CurrentSession.Delete(CurrentSession.Get<PendingInvoiceItem>(id));
		}

		public bool JobItemHasPendingInvoiceItem(Guid jobItemId)
		{
			return CurrentSession.Query<PendingInvoiceItem>().Where(j => j.JobItem.Id == jobItemId).SingleOrDefault() != null;
		}

		public IEnumerable<PendingInvoiceItem> GetPendingInvoiceItems()
		{
			return CurrentSession.Query<PendingInvoiceItem>();
		}

		public IEnumerable<InvoiceItem> GetInvoiceItems(Guid invoiceId)
		{
			return CurrentSession.Query<InvoiceItem>().Where(i => i.Invoice.Id == invoiceId);
		}
	}
}