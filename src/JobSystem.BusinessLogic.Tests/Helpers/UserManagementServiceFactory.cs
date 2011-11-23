using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.Framework.Messaging;
using JobSystem.Framework.Security;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class UserManagementServiceFactory
	{
		public static UserManagementService Create()
		{
			var sessionScopeStub = MockRepository.GenerateStub<IRepositorySessionScope>();
			var sessionFactoryStub = MockRepository.GenerateStub<IRepositorySessionFactory>();
			sessionFactoryStub.Stub(x => x.NewSessionScope()).Return(sessionScopeStub);
			return new UserManagementService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				sessionFactoryStub,
				MockRepository.GenerateStub<IUserAccountRepository>(),
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static UserManagementService Create(IUserAccountRepository userAccountRepository)
		{
			var sessionScopeStub = MockRepository.GenerateStub<IRepositorySessionScope>();
			var sessionFactoryStub = MockRepository.GenerateStub<IRepositorySessionFactory>();
			sessionFactoryStub.Stub(x => x.NewSessionScope()).Return(sessionScopeStub);
			return new UserManagementService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				sessionFactoryStub,
				userAccountRepository,
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}