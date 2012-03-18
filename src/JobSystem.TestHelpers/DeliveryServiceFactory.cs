using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
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
			return new DeliveryService(userContext, deliveryRepository, customerRepository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}