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
			return new UserManagementService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				MockRepository.GenerateStub<IUserAccountRepository>(),
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static UserManagementService Create(IUserAccountRepository userAccountRepository)
		{
			return new UserManagementService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				userAccountRepository,
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}