using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface IInvoiceRepository : IReadWriteRepository<Invoice, Guid>
    {
        int GetInvoicesCount();
        IEnumerable<Invoice> GetInvoices();
        int GetInvoiceItemCount(Guid invoiceId);
    }
}