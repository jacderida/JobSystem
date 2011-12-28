using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
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
			return Create(MockRepository.GenerateStub<IUserAccountRepository>());
		}

		public static UserManagementService Create(IUserAccountRepository userAccountRepository)
		{
			return Create(userAccountRepository, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Admin));
		}

		public static UserManagementService Create(IUserAccountRepository userAccountRepository, IUserContext userContext)
		{
			return new UserManagementService(
				userContext,
				userAccountRepository,
				new CryptographicService(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}