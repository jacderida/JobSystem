using JobSystem.DataModel;
using JobSystem.Framework.Messaging;
using JobSystem.DataModel.Repositories;

namespace JobSystem.BusinessLogic.Services
{
	public class DeliveryItemService : ServiceBase
	{
		private readonly IDeliveryRepository _deliveryRepository;
		private readonly IDeliveryItemRepository _deliveryItemRepository;

		public DeliveryItemService(
			IUserContext applicationContext,
			IDeliveryRepository deliveryRepository,
			IDeliveryItemRepository deliveryItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_deliveryRepository = deliveryRepository;
			_deliveryItemRepository = deliveryItemRepository;
		}
	}
}