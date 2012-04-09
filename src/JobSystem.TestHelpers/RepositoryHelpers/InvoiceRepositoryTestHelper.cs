using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class InvoiceRepositoryTestHelper
	{
		public static IInvoiceRepository GetInvoiceRepository_StubsGetById_ReturnsInvoice(Invoice invoice)
		{
			var invoiceRepositoryStub = MockRepository.GenerateStub<IInvoiceRepository>();
			invoiceRepositoryStub.Stub(x => x.GetById(invoice.Id)).Return(invoice);
			return invoiceRepositoryStub;
		}

		public static IInvoiceRepository GetInvoiceRepository_StubsGetById_ReturnsNull(Guid invoiceId)
		{
			var invoiceRepositoryStub = MockRepository.GenerateStub<IInvoiceRepository>();
			invoiceRepositoryStub.Stub(x => x.GetById(invoiceId)).Return(null);
			return invoiceRepositoryStub;
		}
	}
}