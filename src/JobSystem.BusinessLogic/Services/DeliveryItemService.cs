using JobSystem.DataModel;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class DeliveryItemService : ServiceBase
	{
		public DeliveryItemService(
			IUserContext applicationContext,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
		}
	}
}