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
			IInvoiceItemRepository invoiceItemRepository,
			IJobItemRepository jobItemRepository,
			IQuoteItemRepository quoteItemRepository)
		{
			return new InvoiceItemService(userContext, invoiceItemRepository, jobItemRepository, quoteItemRepository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}