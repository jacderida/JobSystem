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
		private IJobItemRepository _jobItemRepository;
		private InstrumentService _instrumentService;
		private ListItemService _listItemService;

		public JobItemService(
			IUserContext applicationContext,
			IJobRepository jobRepository,
			IJobItemRepository jobItemRepository,
			ListItemService listItemService,
			InstrumentService instrumentService,
			IQueueDispatcher<IMessage> dispatcher)
			: base(applicationContext, dispatcher)
		{
			_jobRepository = jobRepository;
			_instrumentService = instrumentService;
			_listItemService = listItemService;
			_jobItemRepository = jobItemRepository;
		}

		public JobItem CreateJobItem(
			Guid jobId, Guid jobItemId, Guid instrumentId, string serialNo, string assetNo, Guid initialStatusId, Guid fieldId, int calPeriod,
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
				Instrument = _instrumentService.GetById(instrumentId),
				SerialNo = serialNo,
				AssetNo = assetNo,
				InitialStatus = _listItemService.GetById(initialStatusId),
				Status = _listItemService.GetByName("Booked In"),
				Field = _listItemService.GetById(fieldId),
				CalPeriod = ValidateCalPeriod(calPeriod),
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

		public JobItem AddWorkItem(Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId)
		{
			var jobItem = GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid job item ID must be supplied.");
			var status = ValidateWorkStatus(workStatusId);
			var workType = ValidateWorkType(workTypeId);
			workTime = ValidateWorkTime(workTime);
			overTime = ValidateOverTime(overTime);
			report = ValidateReport(report);
			jobItem.Status = status;
			_jobItemRepository.EmitItemHistory(CurrentUser, jobItemId, workTime, overTime, report, status.Type, workType.Type);
			_jobItemRepository.Update(jobItem);
			return jobItem;
		}

		public JobItem GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobItemRepository.GetById(id);
		}

		public IEnumerable<JobItem> GetJobItems(Guid jobId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobItemRepository.GetJobItems(jobId);
		}

		public ConsignmentItem GetLatestConsignmentItem(Guid jobItemId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobItemRepository.GetLatestConsignmentItem(jobItemId);
		}

		public PendingConsignmentItem GetPendingConsignmentItem(Guid jobItemId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobItemRepository.GetPendingConsignmentItem(jobItemId);
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

		private ListItem ValidateWorkStatus(Guid workStatusId)
		{
			var status = _listItemService.GetById(workStatusId);
			if (status.Category.Type != ListItemCategoryType.JobItemWorkStatus)
				throw new DomainValidationException(Messages.InvalidStatusCategory, "WorkStatusId");
			return status;
		}

		private ListItem ValidateWorkType(Guid workTypeId)
		{
			var workType = _listItemService.GetById(workTypeId);
			if (workType.Category.Type != ListItemCategoryType.JobItemWorkType)
				throw new DomainValidationException(Messages.InvalidWorkTypeCategory, "WorkTypeId");
			return workType;
		}

		private int ValidateCalPeriod(int calPeriod)
		{
			if (calPeriod <= 0)
				throw new DomainValidationException(Messages.InvalidCalPeriod, "CalPeriod");
			return calPeriod;
		}

		private int ValidateWorkTime(int workTime)
		{
			if (workTime < 0)
				throw new DomainValidationException(Messages.ItemHistoryInvalidWorkTime, "WorkTime");
			return workTime;
		}

		private int ValidateOverTime(int overTime)
		{
			if (overTime < 0)
				throw new DomainValidationException(Messages.ItemHistoryInvalidOverTime, "OverTime");
			return overTime;
		}

		private string ValidateReport(string report)
		{
			if (String.IsNullOrEmpty(report))
				return String.Empty;
			if (report.Length > 255)
				throw new DomainValidationException(Messages.ItemHistoryReportTooLarge, "Report");
			return report;
		}
	}
}