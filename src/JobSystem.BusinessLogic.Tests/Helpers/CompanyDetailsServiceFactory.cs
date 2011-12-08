using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class CompanyDetailsServiceFactory
	{
		public static CompanyDetailsService Create()
		{
			return new CompanyDetailsService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static CompanyDetailsService Create(ICompanyDetailsRepository repository)
		{
			return new CompanyDetailsService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				repository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}