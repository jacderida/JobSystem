using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class QuoteItemService : ServiceBase
	{
		private readonly IQuoteRepository _quoteRepository;
		private readonly IQuoteItemRepository _quoteItemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IJobItemRepository _jobItemRepository;
		private readonly IListItemRepository _listItemRepository;

		public QuoteItemService(
			IUserContext applicationContext,
			IQuoteRepository quoteRepository,
			IQuoteItemRepository quoteItemRepository,
			IJobItemRepository jobItemRepository,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_quoteRepository = quoteRepository;
			_quoteItemRepository = quoteItemRepository;
			_customerRepository = customerRepository;
			_jobItemRepository = jobItemRepository;
			_listItemRepository = listItemRepository;
		}
	}
}