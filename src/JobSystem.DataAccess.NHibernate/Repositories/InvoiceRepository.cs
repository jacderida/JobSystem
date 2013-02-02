using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public int GetInvoiceItemCount(Guid invoiceId)
        {
            return CurrentSession.Query<InvoiceItem>().Where(i => i.Invoice.Id == invoiceId).Count();
        }

        public int GetInvoicesCount()
        {
            return CurrentSession.Query<Invoice>().Count();
        }

        public IEnumerable<Invoice> GetInvoices()
        {
            return CurrentSession.Query<Invoice>();
        }
    }
}