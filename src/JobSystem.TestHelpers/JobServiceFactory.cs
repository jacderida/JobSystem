using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.TestHelpers.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public class JobServiceFactory
	{
		public static JobService Create(Guid typeId, Guid customerId)
		{
			return Create(MockRepository.GenerateStub<IJobRepository>(), typeId, customerId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobService Create(IJobRepository jobRepository, Guid typeId, Guid customerId)
		{
			return Create(jobRepository, typeId, customerId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobService Create(IJobRepository jobRepository, Guid typeId, Guid customerId, IUserContext userContext)
		{
			return new JobService(userContext, jobRepository, GetListItemRepository(typeId), GetCustomerRepository(customerId), EntityIdProviderFactory.GetEntityIdProviderFor<Job>("2000JR"), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static JobService CreateForApproval(IJobRepository jobRepository, Guid jobId, Guid typeId, Guid customerId, IUserContext userContext)
		{
			if (jobId != Guid.Empty)
				jobRepository.Stub(x => x.GetById(jobId)).Return(GetJob(jobId, customerId, typeId));
			else
				jobRepository.Stub(x => x.GetById(jobId)).Return(null);
			return new JobService(userContext, jobRepository, GetListItemRepository(typeId), GetCustomerRepository(customerId), EntityIdProviderFactory.GetEntityIdProviderFor<Job>("2000JR"), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		private static IListItemRepository GetListItemRepository(Guid typeId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (typeId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(typeId)).Return(GetJobType(typeId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(typeId)).Return(null);
			return listItemRepositoryStub;
		}

		private static ICustomerRepository GetCustomerRepository(Guid customerId)
		{
			var customerRepositoryStub = MockRepository.GenerateStub<ICustomerRepository>();
			if (customerId != Guid.Empty)
				customerRepositoryStub.Stub(x => x.GetById(customerId)).Return(GetCustomer(customerId));
			else
				customerRepositoryStub.Stub(x => x.GetById(customerId)).Return(null);
			return customerRepositoryStub;
		}

		private static ListItem GetJobType(Guid typeId)
		{
			return new ListItem
			{
				Id = typeId,
				Name = "Lab Services",
				Type = ListItemType.JobTypeField
			};
		}

		private static Job GetJob(Guid jobId, Guid customerId, Guid typeId)
		{
			return new Job
			{
				Id = jobId,
				JobNo = "JR2000",
				OrderNo = "ORDER12345",
				AdviceNo = "ADVICE12345",
				Contact = "Graham Robertson",
				CreatedBy = new UserAccount { Id = Guid.NewGuid(), EmailAddress = "graham.robertson@intertek.com", Name = "Graham Robertson", JobTitle = "Laboratory Manager" },
				Customer = GetCustomer(customerId),
				DateCreated = DateTime.Now,
				Type = GetJobType(typeId),
				IsPending = true,
				Instructions = "Job instructions",
				Notes = "job notes"
			};
		}

		private static Customer GetCustomer(Guid customerId)
		{
			return new Customer
			{
				Id = customerId,
				Name = "EMIS (UK) Ltd"
			};
		}
	}
}