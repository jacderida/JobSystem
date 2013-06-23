using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.Orders;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
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
        private PendingOrderItem _savedPendingItem;
        private OrderItem _orderItemForEdit;
        private Guid _orderItemForEditId;
        private PendingOrderItem _pendingItemForEdit;
        private Guid _pendingItemForEditId;

        private Guid _orderItemForMarkReceivedId;
        private OrderItem _orderItemForMarkReceived;
        private Guid _jobItemForMarkReceivedId;
        private JobItem _jobItemForMarkReceived;
        private DateTime _dateReceived = new DateTime(2011, 12, 29);
        private ListItem _partsReceivedListItem;

        [SetUp]
        public void Setup()
        {
            _domainValidationException = null;
            _jobItemToUpdateId = Guid.NewGuid();
            _orderItemService = null;
            _savedOrderItem = null;
            _savedPendingItem = null;
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
                CreatedUser = _userContext.GetCurrentUser()
            };
            _orderItemForEditId = Guid.NewGuid();
            _orderItemForEdit = new OrderItem
            {
                Id = _orderItemForEditId,
                ItemNo = 1,
                PartNo = "P1000",
                DeliveryDays = 20,
                Instructions = "some instructions",
                Quantity = 1,
                Price = 10.99m,
                Description = "some description",
                Order = new Order { Id = Guid.NewGuid() }
            };
            _pendingItemForEditId = Guid.NewGuid();
            _pendingItemForEdit = new PendingOrderItem
            {
                Id = _pendingItemForEditId,
                PartNo = "part no",
                Instructions = "instructions",
                Price = 99.99m,
                Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
                Quantity = 1,
                DeliveryDays = 20,
                Description = "some description",
                JobItem = new JobItem { Id = Guid.NewGuid() }
            };

            _jobItemForMarkReceived = new JobItem
            {
                Id = _jobItemForMarkReceivedId,
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
                Status = new ListItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Awaiting Parts",
                    Type = ListItemType.StatusAwaitingParts,
                    Category = new ListItemCategory
                    {
                        Id = Guid.NewGuid(),
                        Name = "JobItemStatus",
                        Type = ListItemCategoryType.JobItemStatus
                    }
                }
            };

            AppDateTime.GetUtcNow = () => _dateReceived;
            _orderItemForMarkReceivedId = Guid.NewGuid();
            _orderItemForMarkReceived = new OrderItem
            {
                Id = _orderItemForMarkReceivedId,
                ItemNo = 1,
                Description = "ordered item",
                Quantity = 2,
                Price = 35.00m,
                DeliveryDays = 30,
                PartNo = "PART28979",
                Instructions = "some instructions",
                JobItem = _jobItemForMarkReceived
            };
            _partsReceivedListItem = new ListItem
            {
                Id = Guid.NewGuid(),
                Name = "Parts Received",
                Type = ListItemType.StatusPartsReceived,
                Category = new ListItemCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "JobItemStatus",
                    Type = ListItemCategoryType.JobItemStatus
                }
            };
        }

        #region Create

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
            var description = "some description";

            var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
            orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                orderItemRepositoryMock,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price);
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
            var description = "some description";

            var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
            orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith1Item(orderId),
                orderItemRepositoryMock,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price);
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
            var description = "some description";

            var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
            orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
            var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
            jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
            jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
                _userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item on order OR2000", ListItemType.StatusAwaitingParts, ListItemType.WorkTypeAdministration));
            jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                orderItemRepositoryMock,
                MockRepository.GenerateStub<ISupplierRepository>(),
                jobItemRepositoryMock,
                OrderItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusAwaitingParts());
            _jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
            orderItemRepositoryMock.VerifyAllExpectations();
            jobItemRepositoryMock.VerifyAllExpectations();
            Assert.AreNotEqual(Guid.Empty, _savedOrderItem.Id);
            Assert.AreEqual(1, _savedOrderItem.ItemNo);
            Assert.IsNotNull(_savedOrderItem.JobItem);
            Assert.AreEqual(ListItemType.StatusAwaitingParts, _jobItemToUpdate.Status.Type);
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
            var description = "some description";

            var orderItemRepositoryMock = MockRepository.GenerateStub<IOrderItemRepository>();
            orderItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
            var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
            jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
            jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
                _userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item on order OR2000", ListItemType.StatusAwaitingParts, ListItemType.WorkTypeAdministration));
            jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith1Item(orderId),
                orderItemRepositoryMock,
                MockRepository.GenerateStub<ISupplierRepository>(),
                jobItemRepositoryMock,
                OrderItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusAwaitingParts());
            _jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
            orderItemRepositoryMock.VerifyAllExpectations();
            jobItemRepositoryMock.VerifyAllExpectations();
            Assert.AreNotEqual(Guid.Empty, _savedOrderItem.Id);
            Assert.AreEqual(2, _savedOrderItem.ItemNo);
            Assert.IsNotNull(_savedOrderItem.JobItem);
            Assert.AreEqual(ListItemType.StatusAwaitingParts, _jobItemToUpdate.Status.Type);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsNull(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
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
            var description = "some description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidInstructions));
        }

        [Test]
        public void Create_EmptyDescription_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = String.Empty;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.EmptyDescription));
        }

        [Test]
        public void Create_DescriptionGreaterThan255Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = new string('a', 256);

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDescription));
        }

        [Test]
        public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some order description";

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(orderId),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemToUpdateId),
                MockRepository.GenerateStub<IListItemRepository>());
            _jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
            CreateOrderItem(id, orderId, description, quantity, partNo, instructions, deliveryDays, _jobItemToUpdateId, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void CreateOrderItem(Guid id, Guid orderId, string description, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price)
        {
            try
            {
                _savedOrderItem = _orderItemService.Create(id, orderId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price);
                if (jobItemId != Guid.Empty)
                    _jobItemToUpdate = _jobItemService.GetById(jobItemId);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region CreatePending

        [Test]
        public void CreatePending_ValidItemDetails_PendingItemCreated()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryMock = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryMock.Expect(x => x.CreatePending(null)).IgnoreArguments();
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryMock,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            orderItemRepositoryMock.VerifyAllExpectations();
            Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
            Assert.IsNotNull(_savedPendingItem.JobItem);
            Assert.IsNotNull(_savedPendingItem.Supplier);
            Assert.AreEqual(_savedPendingItem.Quantity, quantity);
            Assert.AreEqual(_savedPendingItem.PartNo, partNo);
            Assert.AreEqual(_savedPendingItem.Instructions, instructions);
            Assert.AreEqual(_savedPendingItem.DeliveryDays, deliveryDays);
            Assert.AreEqual(_savedPendingItem.Price, price);
            Assert.AreEqual(_savedPendingItem.Description, description);
            Assert.AreEqual(_savedPendingItem.Carriage, carriage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePending_IdNotSupplied_ArgumentExceptionThrown()
        {
            var id = Guid.Empty;
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePending_InvalidSupplierId_ArgumentExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsNull(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePending_InvalidJobId_ArgumentExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
        }

        [Test]
        public void CreatePending_JobIsPending_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.JobPending));
        }

        [Test]
        public void CreatePending_JobItemAlreadyHasPendingItem_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateStub<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.JobItemHasPendingOrderItem(jobItemId)).Return(true);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.PendingItemExists));
        }

        [Test]
        public void CreatePending_QuantityLessThan1_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 0;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidQuantity));
        }

        [Test]
        public void CreatePending_PartNoGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = new string('a', 51);
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPartNo));
        }

        [Test]
        public void CreatePending_EmptyDescription_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = String.Empty;
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.EmptyDescription));
        }

        [Test]
        public void CreatePending_DescriptionGreaterThan255Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = new string('a', 256);
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDescription));
        }

        [Test]
        public void CreatePending_InstructionsGreaterThan2000Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = new string('a', 2001);
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidInstructions));
        }

        [Test]
        public void CreatePending_PriceLessThan0_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = -29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPrice));
        }

        [Test]
        public void CreatePending_CarriageLessThan0_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = -99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidCarraige));
        }

        [Test]
        public void CreatePending_DeliveryDaysLessThan0_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = -30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDeliveryDays));
        }

        [Test]
        public void CreatePending_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "PART1000";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 29.99m;
            var description = "some description";
            var carriage = 99.99m;

            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
                MockRepository.GenerateStub<IListItemRepository>());
            CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        public void CreatePending(Guid id, Guid supplierId, string description, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price, decimal? carriage)
        {
            try
            {
                _savedPendingItem = _orderItemService.CreatePending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, jobItemId, price, carriage);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region Edit

        [Test]
        public void Edit_ValidItemDetails_ItemSuccessfullyEdited()
        {
            var quantity = 2;
            var partNo = "edited part no";
            var instructions = "some edited instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryMock = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryMock.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            orderItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryMock,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            orderItemRepositoryMock.VerifyAllExpectations();
            Assert.AreEqual(2, _orderItemForEdit.Quantity);
            Assert.AreEqual("edited part no", _orderItemForEdit.PartNo);
            Assert.AreEqual("some edited instructions", _orderItemForEdit.Instructions);
            Assert.AreEqual(30, _orderItemForEdit.DeliveryDays);
            Assert.AreEqual(20.99m, _orderItemForEdit.Price);
            Assert.AreEqual("edited description", _orderItemForEdit.Description);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Edit_InvalidOrderItemId_ArgumentExceptionThrown()
        {
            var quantity = 2;
            var partNo = "edited part no";
            var instructions = "some edited instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(null);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
        }

        [Test]
        public void Edit_QuantityLessThan1_DomainValidationExceptionThrown()
        {
            var quantity = 0;
            var partNo = "edited part no";
            var instructions = "some edited instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidQuantity));
        }

        [Test]
        public void Edit_DeliveryDaysLessThan0_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = "edited part no";
            var instructions = "some edited instructions";
            var deliveryDays = -30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDeliveryDays));
        }

        [Test]
        public void Edit_PriceLessThan0_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = "edited part no";
            var instructions = "some edited instructions";
            var deliveryDays = 30;
            var price = -20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPrice));
        }

        [Test]
        public void Edit_PartNoGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = new string('a', 51);
            var instructions = "some edited instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPartNo));
        }

        [Test]
        public void Edit_InstructionsGreaterThan2000Characters_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = "edited part no";
            var instructions = new string('a', 2001);
            var deliveryDays = 30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidInstructions));
        }

        [Test]
        public void Edit_DescriptionEmpty_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = "edited part no";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = String.Empty;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.EmptyDescription));
        }

        [Test]
        public void Edit_DescriptionGreaterThan255Characters_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = "edited part no";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = new string('a', 256);

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDescription));
        }

        [Test]
        public void Edit_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            var quantity = 1;
            var partNo = "edited part no";
            var instructions = "some instructions";
            var deliveryDays = 30;
            var price = 20.99m;
            var description = "edited description";

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetById(_orderItemForEditId)).Return(_orderItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            Edit(_orderItemForEditId, description, quantity, partNo, instructions, deliveryDays, price);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        public void Edit(Guid id, string description, int quantity, string partNo, string instructions, int deliveryDays, decimal price)
        {
            try
            {
                _orderItemForEdit = _orderItemService.Edit(id, description, quantity, partNo, instructions, deliveryDays, price);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region EditPending

        [Test]
        public void EditPending_ValidItemDetails_ItemEditedSuccessfully()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 3;
            var partNo = "edited part no - edit";
            var instructions = "some instructions - edited";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryMock = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryMock.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            orderItemRepositoryMock.Expect(x => x.UpdatePendingItem(null)).IgnoreArguments();
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryMock,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            orderItemRepositoryMock.VerifyAllExpectations();
            Assert.AreEqual(supplierId, _pendingItemForEdit.Supplier.Id);
            Assert.AreEqual(quantity, _pendingItemForEdit.Quantity);
            Assert.AreEqual(partNo, _pendingItemForEdit.PartNo);
            Assert.AreEqual(instructions, _pendingItemForEdit.Instructions);
            Assert.AreEqual(deliveryDays, _pendingItemForEdit.DeliveryDays);
            Assert.AreEqual(price, _pendingItemForEdit.Price);
            Assert.AreEqual(description, _pendingItemForEdit.Description);
            Assert.AreEqual(carriage, _pendingItemForEdit.Carriage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EditPending_InvalidPendingId_ArgumentExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 3;
            var partNo = "edited part no - edit";
            var instructions = "some instructions - edited";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(null);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EditPending_InvalidSupplierId_ArgumentExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 3;
            var partNo = "edited part no - edit";
            var instructions = "some instructions - edited";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsNull(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
        }

        [Test]
        public void EditPending_InvalidQuantity_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 0;
            var partNo = "edited part no - edit";
            var instructions = "some instructions - edited";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidQuantity));
        }

        [Test]
        public void EditPending_InvalidPartNo_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = new string('a', 51);
            var instructions = "some instructions - edited";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPartNo));
        }

        [Test]
        public void EditPending_EmptyDescription_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = "some instructions";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = String.Empty;
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.EmptyDescription));
        }

        [Test]
        public void EditPending_DescriptionGreaterThan255Characters_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = "some instructions";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = new string('a', 256);
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDescription));
        }

        [Test]
        public void EditPending_InvalidInstructions_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = new string('a', 2001);
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidInstructions));
        }

        [Test]
        public void EditPending_InvalidDeliveryDays_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = "some instructions";
            var deliveryDays = -25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidDeliveryDays));
        }

        [Test]
        public void EditPending_InvalidPrice_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = "some instructions";
            var deliveryDays = 25;
            var price = -20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidPrice));
        }

        [Test]
        public void EditPending_InvalidCarriage_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = "some instructions";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = -99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InvalidCarraige));
        }

        [Test]
        public void EditPending_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            var supplierId = Guid.NewGuid();
            var quantity = 1;
            var partNo = "part no";
            var instructions = "some instructions";
            var deliveryDays = 25;
            var price = 20.99m;
            var description = "some description";
            var carriage = 99.99m;

            var orderItemRepositoryStub = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryStub.Stub(x => x.GetPendingOrderItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryStub,
                SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            EditPending(_pendingItemForEditId, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void EditPending(
            Guid id, Guid supplierId, string description, int quantity, string partNo, string instructions, int deliveryDays, decimal price, decimal? carriage)
        {
            try
            {
                _pendingItemForEdit = _orderItemService.EditPending(id, supplierId, description, quantity, partNo, instructions, deliveryDays, price, carriage);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region GetById

        [Test]
        public void GetById_UserHasInsufficientSecurityClearance_ThrowsDomainValidationException()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetById(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void GetById(Guid id)
        {
            try
            {
                _orderItemService.GetById(id);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region GetOrderItems

        [Test]
        public void GetOrderItems_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetOrderItems(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void GetOrderItems(Guid orderId)
        {
            try
            {
                _orderItemService.GetOrderItems(orderId);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region GetOrderItemsForJobItem

        [Test]
        public void GetOrderItemsForJobItem_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetOrderItemsForJobItem(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void GetOrderItemsForJobItem(Guid jobItemId)
        {
            try
            {
                _orderItemService.GetOrderItemsForJobItem(jobItemId);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region GetPendingOrderItemForJobItem

        [Test]
        public void GetPendingOrderItemForJobItem_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetPendingOrderItemForJobItem(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        [Test]
        public void GetPendingOrderItemForJobItem_InvalidJobItemId_ArgumentExceptionThrown()
        {
            var jobItemId = Guid.NewGuid();
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                OrderItemServiceTestHelper.GetOrderItemRepository_StubsGetPendingOrderItemForJobItem_ReturnsNull(jobItemId),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetPendingOrderItemForJobItem(jobItemId);
            Assert.IsNull(_pendingItemForEdit);
        }

        private void GetPendingOrderItemForJobItem(Guid jobItemId)
        {
            try
            {
                _pendingItemForEdit = _orderItemService.GetPendingOrderItemForJobItem(jobItemId);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region DeletePendingItem

        [Test]
        public void DeletePendingItem_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            DeletePendingItem(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void DeletePendingItem(Guid id)
        {
            try
            {
                _orderItemService.DeletePendingItem(id);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region GetPendingOrderItems

        [Test]
        public void GetPendingOrderItems_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetPendingOrderItems(new List<Guid>());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        [Test]
        public void GetPendingOrderItemsWithIdList_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IOrderRepository>(),
                MockRepository.GenerateStub<IOrderItemRepository>(),
                MockRepository.GenerateStub<ISupplierRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>(),
                MockRepository.GenerateStub<IListItemRepository>());
            GetPendingOrderItems(new List<Guid> { Guid.NewGuid() });
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(OrderItemMessages.InsufficientSecurity));
        }

        private void GetPendingOrderItems(List<Guid> pendingItemIds)
        {
            try
            {
                if (pendingItemIds.Count == 0)
                    _orderItemService.GetPendingOrderItems();
                else
                    _orderItemService.GetPendingOrderItems(pendingItemIds);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region MarkReceived

        [Test]
        public void MarkReceived_ValidOrderItem_ItemMarkedReceived()
        {
            var orderItemRepositoryMock = MockRepository.GenerateMock<IOrderItemRepository>();
            orderItemRepositoryMock.Stub(x => x.GetById(_orderItemForMarkReceivedId)).Return(_orderItemForMarkReceived);
            orderItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
            var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
            jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForMarkReceivedId)).Return(_jobItemForMarkReceived);
            jobItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
            var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusPartsReceived)).Return(_partsReceivedListItem);
            _orderItemService = OrderItemServiceTestHelper.GetOrderItemService(
                _userContext,
                MockRepository.GenerateStub<IOrderRepository>(),
                orderItemRepositoryMock,
                MockRepository.GenerateStub<ISupplierRepository>(),
                jobItemRepositoryMock,
                listItemRepositoryStub);

            MarkReceived(_orderItemForMarkReceivedId);
            orderItemRepositoryMock.VerifyAllExpectations();
            jobItemRepositoryMock.VerifyAllExpectations();
            Assert.AreEqual(_orderItemForMarkReceivedId, _orderItemForMarkReceived.Id);
            Assert.AreEqual(_dateReceived, _orderItemForMarkReceived.DateReceived.Value);
            Assert.AreEqual(ListItemType.StatusPartsReceived, _jobItemForMarkReceived.Status.Type);
        }

        private void MarkReceived(Guid orderItemId)
        {
            try
            {
                _orderItemForMarkReceived = _orderItemService.MarkReceived(orderItemId);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
    }
}