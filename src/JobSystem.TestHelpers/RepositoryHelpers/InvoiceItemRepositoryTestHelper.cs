using System;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class InvoiceItemRepositoryTestHelper
	{
		public static IInvoiceItemRepository GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsTrue(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IInvoiceItemRepository>();
			jobItemRepositoryStub.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(true);
			return jobItemRepositoryStub;
		}

		public static IInvoiceItemRepository GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IInvoiceItemRepository>();
			jobItemRepositoryStub.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(false);
			return jobItemRepositoryStub;
		}
	}
}