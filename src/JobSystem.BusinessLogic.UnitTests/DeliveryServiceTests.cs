using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.Framework;
using JobSystem.Resources.Delivery;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class DeliveryServiceTests
	{
		private IUserContext _userContext;
		private DomainValidationException _domainValidationException;
		private DeliveryService _deliveryService;
		private Delivery _savedDelivery;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		[SetUp]
		public void Setup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_domainValidationException = null;
			_savedDelivery = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
		}

		[Test]
		public void Create_ValidDeliveryDetails_DeliveryCreated()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var fao = "Graham Robertson";

			var deliveryRepositoryMock = MockRepository.GenerateMock<IDeliveryRepository>();
			deliveryRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_deliveryService = DeliveryServiceFactory.Create(
				_userContext, deliveryRepositoryMock, CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			Create(id, customerId, fao);
			deliveryRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedDelivery.Id);
			Assert.AreEqual(_dateCreated, _savedDelivery.DateCreated);
			Assert.IsNotNull(_savedDelivery.Customer);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var customerId = Guid.NewGuid();
			var fao = "Graham Robertson";

			_deliveryService = DeliveryServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			Create(id, customerId, fao);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidCustomerId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var fao = "Graham Robertson";

			_deliveryService = DeliveryServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsNull(customerId));
			Create(id, customerId, fao);
		}

		[Test]
		public void Create_InvalidFao_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var fao = new string('a', 256);

			_deliveryService = DeliveryServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			Create(id, customerId, fao);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidFao));
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var fao = "Graham Robertson";

			_deliveryService = DeliveryServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IDeliveryRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			Create(id, customerId, fao);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void Create(Guid id, Guid customerId, string fao)
		{
			try
			{
				_savedDelivery = _deliveryService.Create(id, customerId, fao);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}