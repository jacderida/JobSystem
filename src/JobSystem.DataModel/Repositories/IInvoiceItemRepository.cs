using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IInvoiceItemRepository : IReadWriteRepository<InvoiceItem, Guid>
	{
		void CreatePendingItem(PendingInvoiceItem pendingItem);
		bool JobItemHasPendingInvoiceItem(Guid jobItemId);
	}
}