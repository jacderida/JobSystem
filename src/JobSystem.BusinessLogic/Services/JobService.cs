using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class JobService : ServiceBase
	{
		private IJobRepository _jobRepository;
		private IListItemRepository _listItemRepository;
		private ICustomerRepository _customerRepository;

		public JobService(
			IUserContext applicationContext,
			IJobRepository jobRepository,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_jobRepository = jobRepository;
			_listItemRepository = listItemRepository;
			_customerRepository = customerRepository;
		}

		public Job CreateJob(
			Guid id, string instructions, string orderNo, string adviceNo, Guid typeId, Guid customerId, string notes, string contact)
		{
			throw new NotImplementedException();
		}
	}
}