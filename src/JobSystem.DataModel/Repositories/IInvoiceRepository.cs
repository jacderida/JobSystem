using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IInvoiceRepository : IReadWriteRepository<Invoice, Guid>
	{
	}
}