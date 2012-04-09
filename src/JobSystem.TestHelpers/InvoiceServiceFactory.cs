using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public static class InvoiceServiceFactory
	{
		public static InvoiceService Create(
			IUserContext userContext,
			IInvoiceRepository invoiceRepository,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IBankDetailsRepository bankDetailsRepository,
			ITaxCodeRepository taxCodeRepository)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new InvoiceService(
				userContext,
				new InvoiceItemService(
					userContext,
					MockRepository.GenerateStub<ICompanyDetailsRepository>(),
					MockRepository.GenerateStub<IInvoiceRepository>(),
					MockRepository.GenerateStub<IInvoiceItemRepository>(),
					MockRepository.GenerateStub<IJobItemRepository>(),
					MockRepository.GenerateStub<IQuoteItemRepository>(),
					dispatcher),
				invoiceRepository,
				EntityIdProviderFactory.GetEntityIdProviderFor<Invoice>("IR2000"),
				listItemRepository,
				customerRepository,
				bankDetailsRepository,
				taxCodeRepository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}