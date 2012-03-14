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

		public static IOrderRepository GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(Guid orderId)
		{
			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(orderId)).Return(GetOrder(orderId));
			return orderRepositoryStub;
		}

		public static IOrderRepository GetOrderRepository_StubsGetById_ReturnsOrderWith1Item(Guid orderId)
		{
			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(orderId)).Return(GetOrderWith1Item(orderId));
			return orderRepositoryStub;
		}

		public static IOrderRepository GetOrderRepository_StubsGetById_ReturnsNull(Guid orderId)
		{
			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(orderId)).Return(null);
			return orderRepositoryStub;
		}

		public static IOrderItemRepository GetOrderItemRepository_StubsGetPendingOrderItemForJobItem_ReturnsNull(Guid jobItemId)
		{
			var orderItemRepositoryStub = MockRepository.GenerateStub<IOrderItemRepository>();
			orderItemRepositoryStub.Stub(x => x.GetPendingOrderItemForJobItem(jobItemId)).Return(null);
			return orderItemRepositoryStub;
		}

		public static IListItemRepository GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusAwaitingParts()
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusAwaitingParts)).Return(GetOrderedListItem());
			return listItemRepositoryStub;
		}

		private static Order GetOrder(Guid orderId)
		{
			return new Order
			{
				Id = orderId,
				CreatedBy = new UserAccount
				{
					Id = Guid.NewGuid(),
					EmailAddress = "chris.oneil@gmail.com",
					Name = "Chris O'Neil",
					JobTitle = "Development Engineer",
					Roles = UserRole.Manager | UserRole.Member,
					PasswordHash = "passwordhash",
					PasswordSalt = "passwordsalt"
				},
				DateCreated = DateTime.UtcNow,
				Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				Instructions = "blah blah",
				OrderNo = "OR2000",
				Currency = new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Gbp",
					Type = ListItemType.CurrencyGbp,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.Currency, Name = "Currency" }
				}
			};
		}

		private static Order GetOrderWith1Item(Guid orderId)
		{
			return new Order
			{
				Id = orderId,
				CreatedBy = new UserAccount
				{
					Id = Guid.NewGuid(),
					EmailAddress = "chris.oneil@gmail.com",
					Name = "Chris O'Neil",
					JobTitle = "Development Engineer",
					Roles = UserRole.Manager | UserRole.Member,
					PasswordHash = "passwordhash",
					PasswordSalt = "passwordsalt"
				},
				DateCreated = DateTime.UtcNow,
				Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				Instructions = "blah blah",
				OrderNo = "OR2000",
				Currency = new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Gbp",
					Type = ListItemType.CurrencyGbp,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.Currency, Name = "Currency" }
				},
				OrderItems = new List<OrderItem>()
				{
					new OrderItem
					{
						Id = Guid.NewGuid(),
						ItemNo = 1,
						Quantity = 1
					}
				}
			};
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

		private static ListItem GetSubContractLocationListItem()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Sub Contract",
				Type = ListItemType.WorkLocationSubContract,
				Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.JobItemLocation, Name = "Job Item Location" }
			};
		}
	}
}