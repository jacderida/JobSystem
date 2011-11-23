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
			var sessionScopeStub = MockRepository.GenerateStub<IRepositorySessionScope>();
			var sessionFactoryStub = MockRepository.GenerateStub<IRepositorySessionFactory>();
			sessionFactoryStub.Stub(x => x.NewSessionScope()).Return(sessionScopeStub);
			return new CustomerService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				sessionFactoryStub,
				MockRepository.GenerateStub<ICustomerRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static CustomerService Create(ICustomerRepository repository)
		{
			var sessionScopeStub = MockRepository.GenerateStub<IRepositorySessionScope>();
			var sessionFactoryStub = MockRepository.GenerateStub<IRepositorySessionFactory>();
			sessionFactoryStub.Stub(x => x.NewSessionScope()).Return(sessionScopeStub);
			return new CustomerService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				sessionFactoryStub,
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}