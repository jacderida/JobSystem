using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.JobItems;

namespace JobSystem.BusinessLogic.Services
{
	public class JobItemService : ServiceBase
	{
		private IJobRepository _jobRepository;
		private IInstrumentRepository _instrumentRepository;
		private IListItemRepository _listItemRepository;
		private IJobItemRepository _jobItemRepository;

		public JobItemService(
			IUserContext applicationContext,
			IJobRepository jobRepository,
			IJobItemRepository jobItemRepository,
			IInstrumentRepository instrumentRepository,
			IListItemRepository listItemRepository,
			IQueueDispatcher<IMessage> dispatcher)
			: base(applicationContext, dispatcher)
		{
			_jobRepository = jobRepository;
			_instrumentRepository = instrumentRepository;
			_listItemRepository = listItemRepository;
			_jobItemRepository = jobItemRepository;
		}

		public JobItem CreateJobItem(
			Guid jobId, Guid jobItemId, Guid instrumentId, string serialNo, string assetNo, Guid initialStatusId, Guid locationId, Guid fieldId, int calPeriod,
			string instructions, string accessories, bool isReturned, string returnReason, string comments)
		{
			if (jobItemId == Guid.Empty)
				throw new ArgumentException("A valid ID must be supplied for the job item ID");
			var jobItem = new JobItem
			{
				Id = jobItemId,
				ItemNo = _jobRepository.GetJobItemCount(jobId) + 1,
				Created = AppDateTime.GetUtcNow(),
				CreatedUser = CurrentUser,
				Instrument = GetInstrument(instrumentId),
				SerialNo = serialNo,
				AssetNo = assetNo,
				InitialStatus = GetListItem(initialStatusId),
				Status = _listItemRepository.GetByName("Booked In"),
				Location = GetListItem(locationId),
				Field = GetListItem(fieldId),
				CalPeriod = GetAndValidateCalPeriod(calPeriod),
				Instructions = instructions,
				Accessories = accessories,
				IsReturned = isReturned,
				ReturnReason = returnReason,
				Comments = comments,
				ProjectedDeliveryDate = AppDateTime.GetUtcNow().AddDays(30)
			};
			ValidateAnnotatedObjectThrowOnFailure(jobItem);
			var job = GetJob(jobId);
			jobItem.Job = job;
			_jobItemRepository.Create(jobItem);
			return jobItem;
		}

		public ItemHistory CreateItemHistory(Guid id, Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.ItemHistoryInsufficientSecurityClearance);
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied to create the work item.");
			var jobItem = GetById(jobItemId);
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

		public JobItem GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException("A member role is required for this operation");
			return _jobItemRepository.GetById(id);
		}

		public IEnumerable<JobItem> GetJobItems(Guid jobId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException("A member role is required for this operation");
			return _jobItemRepository.GetJobItems(jobId);
		}

		private Job GetJob(Guid jobId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException("A member role is required for this operation");
			var job = _jobRepository.GetById(jobId);
			if (job == null)
				throw new ArgumentException("A valid ID must be supplied for the job ID");
			return job;
		}

		private Instrument GetInstrument(Guid instrumentId)
		{
			var instrument = _instrumentRepository.GetById(instrumentId);
			if (instrument == null)
				throw new ArgumentException("A valid ID must be supplied for the instrument ID");
			return instrument;
		}

		private ListItem GetListItem(Guid listItemId)
		{
			var type = _listItemRepository.GetById(listItemId);
			if (type == null)
				throw new ArgumentException("A valid ID must be supplied for the list item ID");
			return type;
		}

		private int GetAndValidateCalPeriod(int calPeriod)
		{
			if (calPeriod <= 0)
				throw new DomainValidationException(Messages.InvalidCalPeriod, "CalPeriod");
			return calPeriod;
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