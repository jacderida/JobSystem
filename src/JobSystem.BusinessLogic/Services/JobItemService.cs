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
				Instrument = _instrumentService.GetById(instrumentId),
				SerialNo = serialNo,
				AssetNo = assetNo,
				InitialStatus = _listItemService.GetById(initialStatusId),
				Status = _listItemService.GetByName("Booked In"),
				Location = _listItemService.GetById(locationId),
				Field = _listItemService.GetById(fieldId),
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

		private Job GetJob(Guid jobId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException("A member role is required for this operation");
			var job = _jobRepository.GetById(jobId);
			if (job == null)
				throw new ArgumentException("A valid ID must be supplied for the job ID");
			return job;
		}

		private int GetAndValidateCalPeriod(int calPeriod)
		{
			if (calPeriod <= 0)
				throw new DomainValidationException(Messages.InvalidCalPeriod, "CalPeriod");
			return calPeriod;
		}
	}
}