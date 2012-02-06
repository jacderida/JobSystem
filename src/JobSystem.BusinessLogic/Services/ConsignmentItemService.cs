using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Repositories;
using JobSystem.DataModel;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class ConsignmentItemService : ServiceBase
	{
		private readonly ListItemService _listItemService;
		private readonly ItemHistoryService _itemHistoryService;
		private readonly IConsignmentRepository _consignmentRepository;
		private readonly IConsignmentItemRepository _consignmentItemRepository;
		

		public ConsignmentItemService(
			JobItemService jobItemRepository,
			ItemHistoryService itemHistoryService,
			ListItemService listItemService,
			IUserContext applicationContext,
			IConsignmentRepository consignmentRepository,
			IConsignmentItemRepository consignmentItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_listItemService = listItemService;
			_itemHistoryService = itemHistoryService;
			_consignmentRepository = consignmentRepository;
			_consignmentItemRepository = consignmentItemRepository;
		}
	}
}