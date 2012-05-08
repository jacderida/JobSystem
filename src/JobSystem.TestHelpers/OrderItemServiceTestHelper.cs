using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.Framework.Messaging;

namespace JobSystem.TestHelpers
{
	public static class OrderItemServiceTestHelper
	{
		public static OrderItemService GetOrderItemService(
			IUserContext userContext, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, ISupplierRepository supplierRepository, IJobItemRepository jobItemRepository, IListItemRepository listItemRepository)
		{
			return new OrderItemService(
				userContext,
				orderRepository,
				orderItemRepository,
				supplierRepository,
				jobItemRepository,
				listItemRepository,
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static IListItemRepository GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusAwaitingParts()
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusAwaitingParts)).Return(GetOrderedListItem());
			return listItemRepositoryStub;
		}

		public static IOrderItemRepository GetOrderItemRepository_StubsGetPendingOrderItemForJobItem_ReturnsNull(Guid jobItemId)
		{
			var orderItemRepositoryStub = MockRepository.GenerateStub<IOrderItemRepository>();
			orderItemRepositoryStub.Stub(x => x.GetPendingOrderItemForJobItem(jobItemId)).Return(null);
			return orderItemRepositoryStub;
		}

		private static ListItem GetOrderedListItem()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Awaiting Parts",
				Type = ListItemType.StatusAwaitingParts,
				Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.JobItemStatus, Name = "Job Item Status" }
			};
		}
	}
}