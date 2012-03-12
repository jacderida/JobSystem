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
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new OrderService(
				userContext,
				orderRepository,
				supplierRepository,
				listItemRepository,
				EntityIdProviderFactory.GetEntityIdProviderFor<Order>("OR2000"),
				new OrderItemService(userContext, orderRepository, MockRepository.GenerateStub<IOrderItemRepository>(), supplierRepository, MockRepository.GenerateStub<IJobItemRepository>(), listItemRepository, dispatcher), 
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				dispatcher);
		}
	}
}