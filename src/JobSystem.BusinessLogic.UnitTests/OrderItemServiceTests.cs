using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Resources.Orders;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class OrderItemServiceTests
	{
		private OrderItem _savedOrderItem;
		private OrderItemService _orderItemService;
		private JobItemService _jobItemService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private Guid _jobItemToUpdateId;
		private JobItem _jobItemToUpdate;

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			_jobItemToUpdateId = Guid.NewGuid();
			_orderItemService = null;
			_savedOrderItem = null;
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_jobItemToUpdate = new JobItem
			{
				Id = _jobItemToUpdateId,
				Job = new Job
				{
					Id = Guid.NewGuid(),
					JobNo = "JR2000",
					CreatedBy = _userContext.GetCurrentUser(),
					OrderNo = "ORDER12345",
					DateCreated = DateTime.UtcNow,
					Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" }
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
				CreatedUser = _userContext.GetCurrentUser(),
			};
		}

		[Test]
		public void Create_ValidOrderItemDetailsOnOrderWith0ItemsWithNoJobItem_OrderItemCreated()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var jobItemId = Guid.Empty;
			var price = 29.99m;

			var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
			orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				orderItemRepositoryMock,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, jobItemId, price);
			orderItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedOrderItem.Id);
			Assert.AreEqual(1, _savedOrderItem.ItemNo);
		}

		[Test]
		public void Create_ValidOrderItemDetailsOnOrderWith1ItemsWithNoJobItem_OrderItemCreated()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var jobItemId = Guid.Empty;
			var price = 29.99m;

			var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
			orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith1Item(orderId),
				orderItemRepositoryMock,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, jobItemId, price);
			orderItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedOrderItem.Id);
			Assert.AreEqual(2, _savedOrderItem.ItemNo);
		}

		[Test]
		public void Create_ValidOrderItemDetailsOnOrderWith0ItemsRelatedToJobItem_OrderItemCreated()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
			orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item on order OR2000", ListItemType.StatusOrdered, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationSubContract));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				orderItemRepositoryMock,
				jobItemRepositoryMock,
				OrderItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusOrderedAndLocationSubContract());
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			orderItemRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedOrderItem.Id);
			Assert.AreEqual(1, _savedOrderItem.ItemNo);
			Assert.IsNotNull(_savedOrderItem.JobItem);
			Assert.AreEqual(ListItemType.StatusOrdered, _jobItemToUpdate.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationSubContract, _jobItemToUpdate.Location.Type);
		}

		[Test]
		public void Create_ValidOrderItemDetailsOnOrderWith1ItemRelatedToJobItem_OrderItemCreated()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
			orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item on order OR2000", ListItemType.StatusOrdered, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationSubContract));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith1Item(orderId),
				orderItemRepositoryMock,
				jobItemRepositoryMock,
				OrderItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusOrderedAndLocationSubContract());
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			orderItemRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedOrderItem.Id);
			Assert.AreEqual(2, _savedOrderItem.ItemNo);
			Assert.IsNotNull(_savedOrderItem.JobItem);
			Assert.AreEqual(ListItemType.StatusOrdered, _jobItemToUpdate.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationSubContract, _jobItemToUpdate.Location.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				MockRepository.GenerateStub<IOrderRepository>(),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidOrderId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsNull(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
		}

		[Test]
		public void Create_JobPending_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.JobPending));
		}

		[Test]
		public void Create_QuantityLessThan1_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 0;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidQuantity));
		}

		[Test]
		public void Create_PriceLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = -29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPrice));
		}

		[Test]
		public void Create_DeliveryDaysLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART0001";
			var instructions = "some instructions";
			var deliveryDays = -30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDeliveryDays));
		}

		[Test]
		public void Create_PartNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = new string('a', 51);
			var instructions = "some instructions";
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPartNo));
		}

		[Test]
		public void Create_InstructionsGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var orderId = Guid.NewGuid();
			var quantity = 1;
			var partNo = "PART1000";
			var instructions = new string('a', 2001);
			var deliveryDays = 30;
			var price = 29.99m;

			_orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
				_userContext,
				OrderItemServiceTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
				MockRepository.GenerateStub<IOrderItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
				MockRepository.GenerateStub<IListItemRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidInstructions));
		}

		private void CreateOrderItem(Guid id, Guid orderId, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price)
		{
			try
			{
				_savedOrderItem = _orderItemService.Create(id, orderId, quantity, partNo, instructions, deliveryDays, jobItemId, price);
				if (jobItemId != Guid.Empty)
					_jobItemToUpdate = _jobItemService.GetById(jobItemId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}