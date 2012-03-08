using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers.Context;
using Rhino.Mocks;
using JobSystem.DataModel;

namespace JobSystem.TestHelpers
{
	public static class QuoteServiceTestHelper
	{
		public static ICustomerRepository GetCustomerRepository_StubsGetById_ReturnsCustomer(Guid customerId)
		{
			var customerRepositoryStub = MockRepository.GenerateStub<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customerId)).Return(GetCustomer(customerId));
			return customerRepositoryStub;
		}

		public static ICustomerRepository GetCustomerRepository_StubsGetById_ReturnsNull(Guid customerId)
		{
			var customerRepositoryStub = MockRepository.GenerateStub<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customerId)).Return(null);
			return customerRepositoryStub;
		}

		public static QuoteService CreateQuoteService(
			IQuoteRepository quoteRepository, ICustomerRepository customerRepository, IListItemRepository listItemRepository, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new QuoteService(
				userContext, quoteRepository, customerRepository, EntityIdProviderFactory.GetEntityIdProviderFor<Quote>("QR2000"), listItemRepository,
				new QuoteItemService(
					userContext, quoteRepository, MockRepository.GenerateStub<IQuoteItemRepository>(),
					MockRepository.GenerateStub<IJobItemRepository>(), MockRepository.GenerateStub<IListItemRepository>(),
					MockRepository.GenerateStub<ICustomerRepository>(), dispatcher), MockRepository.GenerateStub<ICompanyDetailsRepository>(), dispatcher);
		}

		private static Customer GetCustomer(Guid customerId)
		{
			return new Customer
			{
				Id = customerId,
				Name = "EMIS (UK) Ltd"
			};
		}
	}
}