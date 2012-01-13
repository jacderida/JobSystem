using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Framework;
using JobSystem.BusinessLogic.Validation.Core;
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

		private Job GetJob(Guid jobId)
		{
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
	}
}