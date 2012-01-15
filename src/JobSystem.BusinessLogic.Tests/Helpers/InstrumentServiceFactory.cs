using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class InstrumentServiceFactory
	{
		public static InstrumentService Create()
		{
			return Create(
				MockRepository.GenerateStub<IInstrumentRepository>(),
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static InstrumentService Create(IInstrumentRepository repository)
		{
			return Create(repository, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static InstrumentService Create(IInstrumentRepository repository, IUserContext userContext)
		{
			return new InstrumentService(
				userContext,
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static InstrumentService CreateForSearch(IInstrumentRepository repository)
		{
			return new InstrumentService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member),
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}