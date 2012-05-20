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
	}
}