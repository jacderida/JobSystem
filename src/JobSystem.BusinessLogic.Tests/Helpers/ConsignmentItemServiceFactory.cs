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
	public static class ConsignmentItemServiceFactory
	{
		public static ConsignmentItemService Create(
			IConsignmentItemRepository consignmentItemRepository, IJobItemRepository jobItemRepository, Guid consignmentId, IUserContext userContext, int itemCount = 0)
		{
			return new ConsignmentItemService(
				userContext,
				GetConsignmentRepository(consignmentId, itemCount),
				consignmentItemRepository, jobItemRepository, GetListItemRepository(), MockRepository.GenerateMock<IQueueDispatcher<IMessage>>());
		}

		private static IConsignmentRepository GetConsignmentRepository(Guid consignmentId, int itemCount)
		{
			var consignmentRepositoryStub = MockRepository.GenerateStub<IConsignmentRepository>();
			if (consignmentId != Guid.Empty)
			{
				consignmentRepositoryStub.Stub(x => x.GetById(consignmentId)).Return(
					new Consignment
					{
						Id = Guid.NewGuid(),
						ConsignmentNo = "CR2000",
						CreatedBy = TestUserContext.CreateAdminUser(),
						DateCreated = DateTime.UtcNow,
						Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" }
					});
				consignmentRepositoryStub.Stub(x => x.GetConsignmentItemCount(consignmentId)).Return(itemCount);
			}
			else
				consignmentRepositoryStub.Stub(x => x.GetById(consignmentId)).Return(null);
			return consignmentRepositoryStub;
		}

		private static IListItemRepository GetListItemRepository()
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusConsigned)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Consigned",
					Type = ListItemType.StatusConsigned,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Status", Type = ListItemCategoryType.JobItemStatus }
				});
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.WorkLocationSubContract)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Sub Contract",
					Type = ListItemType.WorkLocationSubContract,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Status", Type = ListItemCategoryType.JobItemLocation }
				});
			return listItemRepositoryStub;
		}
	}
}