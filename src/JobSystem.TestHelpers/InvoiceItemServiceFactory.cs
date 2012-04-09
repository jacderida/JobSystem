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
			IInvoiceRepository invoiceRepository,
			IInvoiceItemRepository invoiceItemRepository,
			IJobItemRepository jobItemRepository,
			IQuoteItemRepository quoteItemRepository)
		{
			return new InvoiceItemService(
				userContext,
				companyDetailsRepository,
				invoiceRepository,
				invoiceItemRepository,
				jobItemRepository,
				quoteItemRepository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}