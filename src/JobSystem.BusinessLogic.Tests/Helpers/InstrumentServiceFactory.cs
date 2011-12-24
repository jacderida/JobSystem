using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class InstrumentServiceFactory
	{
		public static InstrumentService Create()
		{
			return new InstrumentService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member),
				MockRepository.GenerateStub<IInstrumentRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static InstrumentService Create(IInstrumentRepository repository)
		{
			return new InstrumentService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member),
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}