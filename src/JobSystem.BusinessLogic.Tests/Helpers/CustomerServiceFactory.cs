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
			return Create(MockRepository.GenerateStub<ICustomerRepository>());
		}

		public static CustomerService Create(ICustomerRepository repository)
		{
			return Create(repository, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static CustomerService Create(ICustomerRepository repository, IUserContext userContext)
		{
			return new CustomerService(userContext, repository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}