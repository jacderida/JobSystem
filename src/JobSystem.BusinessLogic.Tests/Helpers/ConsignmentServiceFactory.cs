using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Repositories;
using JobSystem.DataModel;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Tests.Context;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class ConsignmentServiceFactory
	{
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