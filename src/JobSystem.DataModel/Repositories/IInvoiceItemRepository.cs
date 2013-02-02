using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface IInvoiceItemRepository : IReadWriteRepository<InvoiceItem, Guid>
    {
        void CreatePendingItem(PendingInvoiceItem pendingItem);
        void DeletePendingItem(Guid id);
        bool JobItemHasPendingInvoiceItem(Guid jobItemId);
        int GetInvoiceItemsCount(Guid invoiceId);
        IEnumerable<InvoiceItem> GetInvoiceItems(Guid invoiceId);
        int GetPendingInvoiceItemsCount();
        IEnumerable<PendingInvoiceItem> GetPendingInvoiceItems();
        IEnumerable<PendingInvoiceItem> GetPendingInvoiceItems(IList<Guid> pendingItemIds);
    }
}