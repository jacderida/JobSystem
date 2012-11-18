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
	public class OrderServiceTests
	{
		private OrderService _orderService;
		private Order _savedOrder;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		private Guid _orderForApprovalId;
		private Order _orderForApproval;

		private Order _orderForMarkReceived;
		private Guid _orderForMarkReceivedId;
		private DateTime _markReceivedDate = new DateTime(2012, 12, 29);

		private Order _orderForEdit;
		private Guid _orderForEditId;

		[SetUp]
		public void Setup()
		{
			_savedOrder = null;
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);

			_orderForApprovalId = Guid.NewGuid();
			_orderForApproval = new Order
			{
				Id = _orderForApprovalId,
				OrderNo = "OR2000",
				CreatedBy = _userContext.GetCurrentUser(),
				Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				DateCreated = DateTime.Now,
				Instructions = "blah",
				OrderItems = new List<OrderItem>() { new OrderItem { Id = Guid.NewGuid() }}
			};

			_orderForMarkReceivedId = Guid.NewGuid();
			_orderForMarkReceived = new Order
			{
				Id = _orderForApprovalId,
				OrderNo = "OR2000",
				CreatedBy = _userContext.GetCurrentUser(),
				Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				DateCreated = DateTime.Now,
				Instructions = "blah",
				OrderItems = new List<OrderItem>() { new OrderItem { Id = Guid.NewGuid() } }
			};

			_orderForEditId = Guid.NewGuid();
			_orderForEdit = new Order
			{
				Id = _orderForEditId,
				OrderNo = "OR2000",
				CreatedBy = _userContext.GetCurrentUser(),
				Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				Currency = new Currency { Id = Guid.NewGuid(), Name = "GBP", DisplayMessage = "Prices in GBP" },
				DateCreated = DateTime.Now,
				Instructions = "blah",
				OrderItems = new List<OrderItem>() { new OrderItem { Id = Guid.NewGuid() } }
			};
		}

		#region Create

		[Test]
		public void Create_ValidOrderDetails_OrderCreated()
		{
			var orderId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "some instructions";
			var currencyId = Guid.NewGuid();

			var orderRepositoryMock = MockRepository.GenerateMock<IOrderRepository>();
			orderRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryMock,
				SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				CurrencyRepositoryTestHelper.GetCurrencyRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateOrder(orderId, supplierId, instructions, currencyId);
			orderRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedOrder.Id);
			Assert.That(_savedOrder.OrderNo.StartsWith("OR"));
			Assert.AreEqual("graham.robertson@intertek.com", _savedOrder.CreatedBy.EmailAddress);
			Assert.AreEqual(_dateCreated, _savedOrder.DateCreated);
			Assert.AreEqual(false, _savedOrder.IsApproved);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			var orderId = Guid.Empty;
			var supplierId = Guid.NewGuid();
			var instructions = "some instructions";
			var currencyId = Guid.NewGuid();

			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				CurrencyRepositoryTestHelper.GetCurrencyRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateOrder(orderId, supplierId, instructions, currencyId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidSupplierId_ArgumentExceptionThrown()
		{
			var orderId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "some instructions";
			var currencyId = Guid.NewGuid();

			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsNull(supplierId),
				CurrencyRepositoryTestHelper.GetCurrencyRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateOrder(orderId, supplierId, instructions, currencyId);
		}

		[Test]
		public void Create_InstructionsGreaterThan255Characters_ArgumentExceptionThrown()
		{
			var orderId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = new string('a', 256);
			var currencyId = Guid.NewGuid();

			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				CurrencyRepositoryTestHelper.GetCurrencyRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateOrder(orderId, supplierId, instructions, currencyId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InstructionsTooLarge));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidCurrencyId_ArgumentExceptionThrown()
		{
			var orderId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "some instructions";
			var currencyId = Guid.NewGuid();

			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				CurrencyRepositoryTestHelper.GetCurrencyRepository_StubsGetById_ReturnsNull(currencyId),
				_userContext);
			CreateOrder(orderId, supplierId, instructions, currencyId);
		}

		[Test]
		public void Create_UserHasInsufficentSecurity_DomainValidationExceptionThrown()
		{
			var orderId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "some instructions";
			var currencyId = Guid.NewGuid();

			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				SupplierRepositoryTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				CurrencyRepositoryTestHelper.GetCurrencyRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			CreateOrder(orderId, supplierId, instructions, currencyId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void CreateOrder(Guid id, Guid supplierId, string instructions, Guid currencyId)
		{
			try
			{
				_savedOrder = _orderService.Create(id, supplierId, instructions, currencyId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Approve

		[Test]
		public void Approve_ValidApprovalContext_OrderSuccessfullyApproved()
		{
			var orderRepositoryMock = MockRepository.GenerateMock<IOrderRepository>();
			orderRepositoryMock.Stub(x => x.GetById(_orderForApprovalId)).Return(_orderForApproval);
			orderRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryMock,
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				_userContext);
			Approve(_orderForApprovalId);
			orderRepositoryMock.VerifyAllExpectations();
			Assert.IsTrue(_orderForApproval.IsApproved);
		}

		[Test]
		public void Approve_OrderWithNoItems_DomainValidationExceptionThrown()
		{
			var orderRepositoryStub = MockRepository.GenerateMock<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForApprovalId)).Return(OrderRepositoryTestHelper.GetOrder(_orderForApprovalId));
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				_userContext);
			Approve(_orderForApprovalId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ApprovalWithZeroItems));
		}

		[Test]
		public void Approve_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var orderRepositoryStub = MockRepository.GenerateMock<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForApprovalId)).Return(_orderForApproval);
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			Approve(_orderForApprovalId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Approve_InvalidOrderId_DomainValidationExceptionThrown()
		{
			_orderService = OrderServiceTestHelper.CreateOrderService(
				OrderRepositoryTestHelper.GetOrderRepository_StubsGetById_ReturnsNull(_orderForApprovalId),
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				_userContext);
			Approve(_orderForApprovalId);
		}

		private void Approve(Guid orderId)
		{
			try
			{
				_orderForApproval = _orderService.ApproveOrder(orderId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion.
		#region GetOrders

		[Test]
		public void GetOrders_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			GetOrders();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void GetOrders()
		{
			try
			{
				_orderService.GetOrders();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetById

		[Test]
		public void GetById_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			GetById(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void GetById(Guid id)
		{
			try
			{
				_orderService.GetById(id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Edit

		[Test]
		public void Edit_ValidOrderDetails_EditSuccessful()
		{
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = "some edited instructions";

			var orderRepositoryMock = MockRepository.GenerateMock<IOrderRepository>();
			orderRepositoryMock.Stub(x => x.GetById(_orderForEditId)).Return(_orderForEdit);
			orderRepositoryMock.Expect(x => x.Update(_orderForEdit)).IgnoreArguments();
			var supplierRepositoryStub = MockRepository.GenerateMock<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "Supplier for edit" });
			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(currencyId)).Return(new Currency { Id = currencyId, Name = "USD" });
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryMock, supplierRepositoryStub, currencyRepositoryStub, _userContext);

			Edit(_orderForEditId, supplierId, currencyId, instructions);
			orderRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_orderForEdit.Id, _orderForEditId);
			Assert.AreEqual(_orderForEdit.Supplier.Id, supplierId);
			Assert.AreEqual(_orderForEdit.Currency.Id, currencyId);
			Assert.AreEqual(_orderForEdit.Instructions, instructions);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidOrderId_ThrowsArgumentException()
		{
			_orderForEditId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = "some edited instructions";

			var orderRepositoryStub = MockRepository.GenerateMock<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForEditId)).Return(null);
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				MockRepository.GenerateStub<ISupplierRepository>(),
				MockRepository.GenerateStub<ICurrencyRepository>(),
				_userContext);
			Edit(_orderForEditId, supplierId, currencyId, instructions);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidSupplierId_ThrowsArgumentException()
		{
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = "some edited instructions";

			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForEditId)).Return(_orderForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(null);
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				supplierRepositoryStub,
				MockRepository.GenerateStub<ICurrencyRepository>(),
				_userContext);
			Edit(_orderForEditId, supplierId, currencyId, instructions);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidCurrencyId_ThrowsArgumentException()
		{
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = "some edited instructions";

			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForEditId)).Return(_orderForEdit);
			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(currencyId)).Return(null);
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				MockRepository.GenerateStub<ISupplierRepository>(),
				currencyRepositoryStub,
				_userContext);
			Edit(_orderForEditId, supplierId, currencyId, instructions);
		}

		[Test]
		public void Edit_InstructionsTooLarge_ThrowsDomainValidationException()
		{
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = new string('a', 256);

			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForEditId)).Return(_orderForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "Supplier for edit" });
			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(currencyId)).Return(new Currency { Id = currencyId, Name = "USD" });
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				supplierRepositoryStub,
				currencyRepositoryStub,
				_userContext);
			Edit(_orderForEditId, supplierId, currencyId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InstructionsTooLarge));
		}

		[Test]
		public void Edit_IsApprovedOrder_ThrowsDomainValidationException()
		{
			_orderForEdit.IsApproved = true;
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = "some edited instructions";

			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForEditId)).Return(_orderForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "Supplier for edit" });
			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(currencyId)).Return(new Currency { Id = currencyId, Name = "USD" });
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				supplierRepositoryStub,
				currencyRepositoryStub,
				_userContext);
			Edit(_orderForEditId, supplierId, currencyId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.EditApprovedOrder));
		}

		[Test]
		public void Edit_UserHasInsufficientSecurityClearance_ThrowsDomainValidationException()
		{
			var supplierId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var instructions = "some edited instructions";

			var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
			orderRepositoryStub.Stub(x => x.GetById(_orderForEditId)).Return(_orderForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "Supplier for edit" });
			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(currencyId)).Return(new Currency { Id = currencyId, Name = "USD" });
			_orderService = OrderServiceTestHelper.CreateOrderService(
				orderRepositoryStub,
				supplierRepositoryStub,
				currencyRepositoryStub,
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			Edit(_orderForEditId, supplierId, currencyId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void Edit(Guid orderId, Guid supplierId, Guid currencyId, string instructions)
		{
			try
			{
				_orderForEdit = _orderService.Edit(orderId, supplierId, currencyId, instructions);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}