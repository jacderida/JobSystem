using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using Rhino.Mocks;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Tests.Context;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class CustomerServiceFactory
	{
		public static CustomerService Create()
		{
			return new CustomerService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				MockRepository.GenerateStub<ICustomerRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static CustomerService Create(ICustomerRepository repository)
		{
			return new CustomerService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}