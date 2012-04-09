using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IInvoiceRepository : IReadWriteRepository<Invoice, Guid>
	{
		int GetInvoiceItemCount(Guid invoiceId);
		IEnumerable<Invoice> GetInvoices();
	}
}