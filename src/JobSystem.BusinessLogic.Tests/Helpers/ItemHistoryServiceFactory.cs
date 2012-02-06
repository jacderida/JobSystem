using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Repositories;
using JobSystem.DataModel;
using Rhino.Mocks;
using JobSystem.Framework.Messaging;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public static class ItemHistoryServiceFactory
	{
		public static ItemHistoryService Create(IJobItemRepository jobItemRepository, Guid workStatusId, Guid workLocationId, Guid workTypeId, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new ItemHistoryService(
				userContext, jobItemRepository,
				new ListItemService(userContext, GetListItemRepositoryForItemHistory(workStatusId, workTypeId, workLocationId), dispatcher),
				dispatcher);
		}

		public static IListItemRepository GetListItemRepositoryForItemHistory(Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (workStatusId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(GetItemHistoryWorkStatus(workStatusId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(null);
			if (workTypeId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(GetItemHistoryWorkType(workTypeId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(null);
			if (workLocationId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(workLocationId)).Return(GetItemHistoryWorkLocation(workLocationId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(workLocationId)).Return(null);
			return listItemRepositoryStub;
		}

		private static ListItem GetItemHistoryWorkStatus(Guid statusId)
		{
			return new ListItem
			{
				Id = statusId,
				Name = "Calibrated",
				Type = ListItemType.JobItemWorkStatus
			};
		}

		private static ListItem GetItemHistoryWorkType(Guid workTypeId)
		{
			return new ListItem
			{
				Id = workTypeId,
				Name = "Calibration",
				Type = ListItemType.JobItemWorkType
			};
		}

		private static ListItem GetItemHistoryWorkLocation(Guid workLocationId)
		{
			return new ListItem
			{
				Id = workLocationId,
				Name = "Calibrated",
				Type = ListItemType.JobItemLocation
			};
		}
	}
}