using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Repositories;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
	{
		public void CreatePendingItem(PendingInvoiceItem pendingItem)
		{
			CurrentSession.Save(pendingItem);
		}
	}
}