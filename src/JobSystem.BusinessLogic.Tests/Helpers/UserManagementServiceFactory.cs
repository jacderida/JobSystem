using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Framework.Security;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class UserManagementServiceFactory
	{
		public static UserManagementService Create()
		{
			return new UserManagementService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Admin),
				MockRepository.GenerateStub<IUserAccountRepository>(),
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static UserManagementService Create(IUserAccountRepository userAccountRepository)
		{
			return new UserManagementService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Admin),
				userAccountRepository,
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}