using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class ConsignmentServiceFactory
	{
		public static ConsignmentService Create(Guid supplierId)
		{
			return Create(MockRepository.GenerateStub<IConsignmentRepository>(), supplierId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static ConsignmentService Create(Guid supplierId, IUserContext userContext)
		{
			return Create(MockRepository.GenerateStub<IConsignmentRepository>(), supplierId, userContext);
		}

		public static ConsignmentService Create(IUserContext userContext)
		{
			return Create(MockRepository.GenerateStub<IConsignmentRepository>(), Guid.NewGuid(), userContext);
		}

		public static ConsignmentService Create(IConsignmentRepository consignmentRepository, Guid supplierId)
		{
			return Create(consignmentRepository, supplierId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static ConsignmentService Create(IConsignmentRepository consignmentRepository, Guid supplierId, IUserContext userContext)
		{
			return new ConsignmentService(userContext, consignmentRepository, GetSupplierRepository(supplierId), EntityIdProviderFactory.GetEntityIdProviderFor<Consignment>("CR2000"), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		private static ISupplierRepository GetSupplierRepository(Guid supplierId)
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			if (supplierId != Guid.Empty)
				supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(GetSupplier(supplierId));
			else
				supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(null);
			return supplierRepositoryStub;
		}

		private static Supplier GetSupplier(Guid supplierId)
		{
			return new Supplier
			{
				Id = supplierId,
				Name = "EMIS (UK) Ltd"
			};
		}
	}
}