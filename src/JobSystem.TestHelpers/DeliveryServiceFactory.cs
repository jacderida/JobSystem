using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public static class DeliveryServiceFactory
	{
		public static DeliveryService Create(
			IUserContext userContext,
			IDeliveryRepository deliveryRepository,
			ICustomerRepository customerRepository)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new DeliveryService(
				userContext,
				deliveryRepository,
				new DeliveryItemService(
					userContext, deliveryRepository, MockRepository.GenerateStub<IDeliveryItemRepository>(), MockRepository.GenerateStub<IJobItemRepository>(), MockRepository.GenerateStub<IQuoteItemRepository>(), MockRepository.GenerateStub<IListItemRepository>(), MockRepository.GenerateStub<ICustomerRepository>(), dispatcher),
				customerRepository,
				EntityIdProviderFactory.GetEntityIdProviderFor<Delivery>("DR2000"),
				dispatcher);
		}
	}
}