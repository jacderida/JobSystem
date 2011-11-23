using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	/// <summary>
	/// A base service class that contains information about the user context.
	/// All service implementations are intended to inherit from this class.
	/// </summary>
	[AutoEnlistRepositorySession]
	public class ServiceBase
	{
		private readonly IUserContext _applicationContext;
		/// <summary>
		/// The Respository Session Factory passed in at construction time.
		/// </summary>
		internal readonly IRepositorySessionFactory RepositorySessionFactory;

		/// <summary>
		/// A simple interface to queue asyncronous messages to be processed on the background queue.
		/// </summary>
		protected readonly IQueueDispatcher<IMessage> _dispatcher;

		/// <summary>
		/// Initialises an instance of the service bass class.
		/// </summary>
		/// <param name="applicationContext">The application context.</param>
		/// <param name="repositorySessionFactory">A factory the service can use to create a repository session with method scope.</param>
		/// <param name="queueDispatcher">A queue dispatcher for sending messages to an asynch queue.</param>
		public ServiceBase(IUserContext applicationContext, IRepositorySessionFactory repositorySessionFactory, IQueueDispatcher<IMessage> queueDispatcher)
		{
			Check.NotNull(applicationContext, "applicationContext");
			Check.NotNull(repositorySessionFactory, "repositorySessionFactory");
			Check.NotNull(queueDispatcher, "queueDispatcher");
			_applicationContext = applicationContext;
			RepositorySessionFactory = repositorySessionFactory;
			_dispatcher = queueDispatcher;
		}

		/// <summary>
		/// Gets or sets an outer repository session scope so that multiple services can take part in the same transaction if required. (such as for test.)
		/// This should be set immediately before any actions are performed on the service.
		/// </summary>
		/// <value>The repository session scope.</value>
		[AutoEnlistRepositorySession(AttributeExclude = true)]
		public IRepositorySessionScope RepositorySessionScope { get; set; }

		/// <summary>
		/// Gets the user context.
		/// </summary>
		[AutoEnlistRepositorySession(AttributeExclude = true)]
		public UserAccount CurrentUser
		{
			get { return _applicationContext.GetCurrentUser(); }
		}

		/// <summary>
		/// Enqueues an asyncronous message for handling later.
		/// </summary>
		/// <param name="message">The message.</param>
		[AutoEnlistRepositorySession(AttributeExclude = true)]
		public void Notify(IMessage message)
		{
			_dispatcher.Enqueue(message);
		}
	}
}