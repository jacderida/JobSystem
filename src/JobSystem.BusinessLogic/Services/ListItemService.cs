using System.Collections.Generic;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using System;

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

		public ListItem GetById(Guid id)
		{
			return _listItemRepository.GetById(id);
		}

		public IEnumerable<ListItem> GetAllByType(ListItemType type)
		{
			return _listItemRepository.GetAllByType(type);
		}
	}
}