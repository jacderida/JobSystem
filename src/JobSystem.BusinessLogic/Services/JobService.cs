using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Framework;
using JobSystem.Resources.Jobs;

namespace JobSystem.BusinessLogic.Services
{
	public class JobService : ServiceBase
	{
		private IJobRepository _jobRepository;
		private IListItemRepository _listItemRepository;
		private ICustomerRepository _customerRepository;
		private IEntityIdProvider _entityIdProvider;

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