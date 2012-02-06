using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.JobItems;

namespace JobSystem.BusinessLogic.Services
{
	public class ItemHistoryService : ServiceBase
	{
		private IJobItemRepository _jobItemRepository;
		private IListItemRepository _listItemRepository;

		public ItemHistoryService(
			IUserContext applicationContext,
			IJobItemRepository jobItemRepository,
			IListItemRepository listItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_jobItemRepository = jobItemRepository;
			_listItemRepository = listItemRepository;
		}

		public ItemHistory CreateItemHistory(Guid id, Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.ItemHistoryInsufficientSecurityClearance);
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied to create the work item.");
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid job item ID must be supplied.");
			var itemHistory = new ItemHistory
			{
				Id = id,
				DateCreated = AppDateTime.GetUtcNow(),
				JobItem = jobItem,
				User = CurrentUser,
				WorkTime = GetAndValidateWorkTime(workTime),
				OverTime = GetAndValidateOverTime(overTime),
				Report = report,
				Status = GetListItem(workStatusId),
				WorkLocation = GetListItem(workLocationId),
				WorkType = GetListItem(workTypeId)
			};
			jobItem.Status = GetListItem(workStatusId);
			jobItem.Location = GetListItem(workLocationId);
			ValidateAnnotatedObjectThrowOnFailure(itemHistory);
			_jobItemRepository.CreateItemHistory(itemHistory);
			_jobItemRepository.Update(jobItem);
			return itemHistory;
		}

		private ListItem GetListItem(Guid listItemId)
		{
			var type = _listItemRepository.GetById(listItemId);
			if (type == null)
				throw new ArgumentException("A valid ID must be supplied for the list item ID");
			return type;
		}

		private int GetAndValidateWorkTime(int workTime)
		{
			if (workTime < 0)
				throw new DomainValidationException(Messages.ItemHistoryInvalidWorkTime, "WorkTime");
			return workTime;
		}

		private int GetAndValidateOverTime(int overTime)
		{
			if (overTime < 0)
				throw new DomainValidationException(Messages.ItemHistoryInvalidOverTime, "OverTime");
			return overTime;
		}
	}
}