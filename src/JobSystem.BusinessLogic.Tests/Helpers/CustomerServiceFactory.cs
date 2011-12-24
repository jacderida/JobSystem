using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using Rhino.Mocks;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class CustomerServiceFactory
	{
		public static CustomerService Create()
		{
			return new CustomerService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member),
				MockRepository.GenerateStub<ICustomerRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static CustomerService Create(ICustomerRepository repository)
		{
			return new CustomerService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member),
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}