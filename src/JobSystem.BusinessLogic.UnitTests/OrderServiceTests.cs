using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using NUnit.Framework;
using JobSystem.BusinessLogic.Services;
using JobSystem.Framework;
using JobSystem.TestHelpers.Context;
using JobSystem.DataModel.Entities;
using Rhino.Mocks;
using JobSystem.DataModel.Repositories;
using JobSystem.TestHelpers;
using JobSystem.Resources.Orders;

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

		[SetUp]
		public void Setup()
		{
			_savedOrder = null;
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
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
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
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
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
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
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsNull(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
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
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
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
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNull(currencyId),
				_userContext);
			CreateOrder(orderId, supplierId, instructions, currencyId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidCurrencyNonCurrencyListItem_ArgumentExceptionThrown()
		{
			var orderId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "some instructions";
			var currencyId = Guid.NewGuid();

			_orderService = OrderServiceTestHelper.CreateOrderService(
				MockRepository.GenerateStub<IOrderRepository>(),
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonCurrencyListItem(currencyId),
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
				OrderServiceTestHelper.GetSupplierRepository_GetById_ReturnsSupplier(supplierId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
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
	}
}