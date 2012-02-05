using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Jobs;

namespace JobSystem.BusinessLogic.Services
{
	public class JobService : ServiceBase
	{
		private readonly IJobRepository _jobRepository;
		private readonly IListItemRepository _listItemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IEntityIdProvider _entityIdProvider;

		public JobService(
			IUserContext applicationContext,
			IJobRepository jobRepository,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IEntityIdProvider entityIdProvider,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_jobRepository = jobRepository;
			_listItemRepository = listItemRepository;
			_customerRepository = customerRepository;
			_entityIdProvider = entityIdProvider;
		}

		public Job CreateJob(Guid id, string instructions, string orderNo, string adviceNo, Guid typeId, Guid customerId, string notes, string contact)
		{
			var job = new Job();
			job.Id = id;
			job.CreatedBy = CurrentUser;
			job.DateCreated = AppDateTime.GetUtcNow();
			job.Instructions = instructions;
			job.OrderNo = orderNo;
			job.AdviceNo = adviceNo;
			job.Customer = GetCustomer(customerId);
			job.Type = GetType(typeId);
			job.JobNo = _entityIdProvider.GetIdFor<Job>();
			job.Notes = notes;
			job.Contact = contact;
			job.IsPending = true;
			ValidateAnnotatedObjectThrowOnFailure(job);
			_jobRepository.Create(job);
			return job;
		}

		public Job ApproveJob(Guid jobId)
		{
			if (!CurrentUser.HasRole(UserRole.JobApprover))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var job = _jobRepository.GetById(jobId);
			if (job == null)
				throw new ArgumentException("A valid ID must be supplied by for job.");
			job.IsPending = false;
			_jobRepository.Update(job);
			return job;
		}

		public Job GetJob(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobRepository.GetById(id);
		}

		public IEnumerable<Job> GetApprovedJobs()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobRepository.GetApprovedJobs().OrderBy(j => j.JobNo);
		}

		public IEnumerable<Job> GetPendingJobs()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _jobRepository.GetPendingJobs().OrderBy(j => j.JobNo);
		}

		private Customer GetCustomer(Guid customerId)
		{
			var customer = _customerRepository.GetById(customerId);
			if (customer == null)
				throw new ArgumentException(Messages.InvalidCustomerId);
			return customer;
		}

		private ListItem GetType(Guid typeId)
		{
			var type = _listItemRepository.GetById(typeId);
			if (type == null)
				throw new ArgumentException(Messages.InvalidTypeId);
			return type;
		}
	}
}