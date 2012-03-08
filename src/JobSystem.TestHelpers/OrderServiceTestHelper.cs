using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.Framework.Messaging;

namespace JobSystem.TestHelpers
{
	public static class OrderServiceTestHelper
	{
		public static OrderService CreateOrderService(
			IOrderRepository orderRepository, ISupplierRepository supplierRepository, IListItemRepository listItemRepository, IUserContext userContext)
		{
			return new OrderService(
				userContext, orderRepository, supplierRepository, listItemRepository,
				EntityIdProviderFactory.GetEntityIdProviderFor<Order>("OR2000"), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static ISupplierRepository GetSupplierRepository_GetById_ReturnsSupplier(Guid supplierId)
		{
			var supplierRepository = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepository.Stub(x => x.GetById(supplierId)).Return(GetSupplier(supplierId));
			return supplierRepository;
		}

		public static ISupplierRepository GetSupplierRepository_GetById_ReturnsNull(Guid supplierId)
		{
			var supplierRepository = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepository.Stub(x => x.GetById(supplierId)).Return(null);
			return supplierRepository;
		}

		private static Supplier GetSupplier(Guid supplierId)
		{
			return new Supplier
			{
				Id = supplierId,
				Name = "Gael Ltd"
			};
		}
	}
}