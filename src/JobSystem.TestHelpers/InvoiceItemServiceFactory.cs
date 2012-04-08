using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public class InvoiceItemServiceFactory
	{
		public static InvoiceItemService Create(
			IUserContext userContext,
			ICompanyDetailsRepository companyDetailsRepository,
			IInvoiceItemRepository invoiceItemRepository,
			IJobItemRepository jobItemRepository,
			IQuoteItemRepository quoteItemRepository)
		{
			return new InvoiceItemService(
				userContext,
				companyDetailsRepository,
				invoiceItemRepository,
				jobItemRepository,
				quoteItemRepository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}