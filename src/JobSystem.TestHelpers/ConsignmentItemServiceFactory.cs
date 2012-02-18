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
	public static class ConsignmentItemServiceFactory
	{
		public static ConsignmentItemService Create(
			IConsignmentItemRepository consignmentItemRepository, IJobItemRepository jobItemRepository, Guid consignmentId, IUserContext userContext, int itemCount = 0)
		{
			return new ConsignmentItemService(
				userContext,
				GetConsignmentRepository(consignmentId, itemCount),
				consignmentItemRepository,
				jobItemRepository,
				GetListItemRepository(),
				MockRepository.GenerateMock<ISupplierRepository>(),
				MockRepository.GenerateMock<IQueueDispatcher<IMessage>>());
		}

		public static ConsignmentItemService CreateForPendingItems(Guid jobItemId, Guid supplierId, IUserContext userContext, bool jobIsPending = false)
		{
			return new ConsignmentItemService(
				userContext,
				MockRepository.GenerateStub<IConsignmentRepository>(),
				GetConsignmentItemRepository(),
				GetJobItemRepository(jobItemId, userContext, jobIsPending),
				MockRepository.GenerateStub<IListItemRepository>(),
				GetSupplierRepository(supplierId),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static ConsignmentItemService CreateForPendingItems(IConsignmentItemRepository consignmentItemRepository, Guid jobItemId, Guid supplierId, IUserContext userContext, bool jobIsPending = false)
		{
			return new ConsignmentItemService(
				userContext,
				MockRepository.GenerateStub<IConsignmentRepository>(),
				consignmentItemRepository,
				GetJobItemRepository(jobItemId, userContext, jobIsPending),
				MockRepository.GenerateStub<IListItemRepository>(),
				GetSupplierRepository(supplierId),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static ConsignmentItemService EditForPendingItems(PendingConsignmentItem pendingItem, Guid jobItemId, Guid supplierId, IUserContext userContext, bool jobIsPending = false)
		{
			return new ConsignmentItemService(
				userContext,
				MockRepository.GenerateStub<IConsignmentRepository>(),
				GetConsignmentItemRepository(pendingItem),
				GetJobItemRepository(jobItemId, userContext, jobIsPending),
				MockRepository.GenerateStub<IListItemRepository>(),
				GetSupplierRepository(supplierId),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static ConsignmentItemService CreateForEditForPendingItems(IConsignmentItemRepository consignmentItemRepository, Guid jobItemId, Guid supplierId, IUserContext userContext, bool jobIsPending = false)
		{
			return new ConsignmentItemService(
				userContext,
				MockRepository.GenerateStub<IConsignmentRepository>(),
				consignmentItemRepository,
				GetJobItemRepository(jobItemId, userContext, jobIsPending),
				MockRepository.GenerateStub<IListItemRepository>(),
				GetSupplierRepository(supplierId),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		private static IConsignmentItemRepository GetConsignmentItemRepository(PendingConsignmentItem pendingItem)
		{
			var consignmentItemRepositoryStub = MockRepository.GenerateStub<IConsignmentItemRepository>();
			if (pendingItem != null)
				consignmentItemRepositoryStub.Stub(x => x.GetPendingItem(pendingItem.Id)).Return(pendingItem);
			else
				consignmentItemRepositoryStub.Stub(x => x.GetPendingItem(Guid.Empty)).IgnoreArguments().Return(null);
			return consignmentItemRepositoryStub;
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

		private static ISupplierRepository GetSupplierRepository(Guid supplierId)
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			if (supplierId != Guid.Empty)
				supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "Gael Ltd" });
			else
				supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(null);
			return supplierRepositoryStub;
		}

		private static IConsignmentItemRepository GetConsignmentItemRepository()
		{
			var consignmentItemRepositoryStub = MockRepository.GenerateStub<IConsignmentItemRepository>();
			consignmentItemRepositoryStub.Stub(x => x.JobItemHasPendingConsignmentItem(Guid.Empty)).IgnoreArguments().Return(false);
			return consignmentItemRepositoryStub;
		}

		private static IJobItemRepository GetJobItemRepository(Guid jobItemId, IUserContext userContext, bool isJobPending)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			if (jobItemId != Guid.Empty)
				jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(
					new JobItem
					{
						Id = jobItemId,
						Job = new Job
						{
							Id = Guid.NewGuid(),
							JobNo = "JR2000",
							CreatedBy = userContext.GetCurrentUser(),
							OrderNo = "ORDER12345",
							DateCreated = DateTime.UtcNow,
							Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
							IsPending = isJobPending
						},
						ItemNo = 1,
						SerialNo = "12345",
						Instrument = new Instrument
						{
							Id = Guid.NewGuid(),
							Manufacturer = "Druck",
							ModelNo = "DPI601IS",
							Range = "None",
							Description = "Digital Pressure Indicator"
						},
						CalPeriod = 12,
						Created = DateTime.UtcNow,
						CreatedUser = userContext.GetCurrentUser(),
					});
			else
				jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(null);
			return jobItemRepositoryStub;
		}
	}
}