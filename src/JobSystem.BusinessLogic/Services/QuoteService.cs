using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class QuoteService : ServiceBase
	{
		private readonly IQuoteRepository _quoteRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly QuoteItemService _quoteItemService;

		public QuoteService(
			IUserContext applicationContext,
			IQuoteRepository quoteRepository,
			ICustomerRepository customerRepository,
			IEntityIdProvider entityIdProvider,
			QuoteItemService quoteItemService,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_quoteRepository = quoteRepository;
			_customerRepository = customerRepository;
			_entityIdProvider = entityIdProvider;
			_quoteItemService = quoteItemService;
		}
	}
}