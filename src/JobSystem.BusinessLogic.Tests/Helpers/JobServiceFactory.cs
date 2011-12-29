using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
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
			return new JobService(userContext, jobRepository, GetListItemRepository(typeId), GetCustomerRepository(customerId), GetEntityIdProvider(), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
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

		private static IEntityIdProvider GetEntityIdProvider()
		{
			var entityIdRepositoryStub = MockRepository.GenerateStub<IEntityIdProvider>();
			entityIdRepositoryStub.Stub(x => x.GetIdFor<Job>()).Return("2000JR");
			return entityIdRepositoryStub;
		}

		private static ListItem GetJobType(Guid typeId)
		{
			return new ListItem
			{
				Id = typeId,
				Name = "Lab Services",
				Type = ListItemType.JobType
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