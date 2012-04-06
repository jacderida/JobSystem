using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IInvoiceItemRepository : IReadWriteRepository<InvoiceItem, Guid>
	{
	}
}
