using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class ServiceBase
	{
		private readonly IUserContext _applicationContext;		
		protected readonly IQueueDispatcher<IMessage> _dispatcher;

		public ServiceBase(IUserContext applicationContext, IQueueDispatcher<IMessage> queueDispatcher)
		{
			Check.NotNull(applicationContext, "applicationContext");
			Check.NotNull(queueDispatcher, "queueDispatcher");
			_applicationContext = applicationContext;
			_dispatcher = queueDispatcher;
		}

		public UserAccount CurrentUser
		{
			get { return _applicationContext.GetCurrentUser(); }
		}

		public void Notify(IMessage message)
		{
			_dispatcher.Enqueue(message);
		}
	}
}