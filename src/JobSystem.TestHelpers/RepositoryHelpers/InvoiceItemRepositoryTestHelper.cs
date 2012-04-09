using System;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class InvoiceItemRepositoryTestHelper
	{
		public static IInvoiceItemRepository GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsTrue(Guid jobItemId)
		{
			var invoiceItemRepositoryStub = MockRepository.GenerateStub<IInvoiceItemRepository>();
			invoiceItemRepositoryStub.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(true);
			return invoiceItemRepositoryStub;
		}

		public static IInvoiceItemRepository GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(Guid jobItemId)
		{
			var invoiceItemRepositoryStub = MockRepository.GenerateStub<IInvoiceItemRepository>();
			invoiceItemRepositoryStub.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(false);
			return invoiceItemRepositoryStub;
		}
	}
}