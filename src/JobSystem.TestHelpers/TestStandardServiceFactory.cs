using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public static class TestStandardServiceFactory
	{
		public static TestStandardsService Create(IUserContext userContext, ITestStandardRepository testStandardRepository)
		{
			return new TestStandardsService(userContext, testStandardRepository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}