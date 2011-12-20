using System.Collections.Generic;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class ListItemService : ServiceBase
	{
		private readonly IListItemRepository _listItemRepository;

		public ListItemService(
			IUserContext applicationContext,
			IListItemRepository listItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_listItemRepository = listItemRepository;
		}

		public IEnumerable<TaxCode> GetTaxCodes()
		{
			return _listItemRepository.GetTaxCodes();
		}

		public IEnumerable<BankDetails> GetBankDetails()
		{
			return _listItemRepository.GetBankDetails();
		}

		public IEnumerable<ListItem> GetAllByType(ListItemType type)
		{
			return _listItemRepository.GetAllByType(type);
		}
	}
}