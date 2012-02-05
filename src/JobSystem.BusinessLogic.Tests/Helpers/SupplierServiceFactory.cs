using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class SupplierServiceFactory
	{
		public static SupplierService Create()
		{
			return Create(MockRepository.GenerateStub<ISupplierRepository>());
		}

		public static SupplierService Create(IUserContext userContext)
		{
			return Create(MockRepository.GenerateStub<ISupplierRepository>(), userContext);
		}

		public static SupplierService Create(ISupplierRepository repository)
		{
			return Create(repository, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static SupplierService Create(ISupplierRepository repository, IUserContext userContext)
		{
			return new SupplierService(userContext, repository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}