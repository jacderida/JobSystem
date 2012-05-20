using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public static class OrderServiceTestHelper
	{
		public static OrderService CreateOrderService(
			IOrderRepository orderRepository, ISupplierRepository supplierRepository, ICurrencyRepository currencyRepository, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new OrderService(
				userContext,
				orderRepository,
				MockRepository.GenerateStub<IConsignmentRepository>(),
				supplierRepository,
				currencyRepository,
				EntityIdProviderFactory.GetEntityIdProviderFor<Order>("OR2000"),
				new OrderItemService(userContext, orderRepository, MockRepository.GenerateStub<IOrderItemRepository>(), supplierRepository, MockRepository.GenerateStub<IJobItemRepository>(), MockRepository.GenerateStub<IListItemRepository>(), dispatcher), 
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				dispatcher);
		}
	}
}