using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.Delivery;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;

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
		private Delivery _deliveryToEdit;
		private Guid _deliveryToEditId;

		[SetUp]
		public void Setup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_domainValidationException = null;
			_savedDelivery = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_deliveryToEditId = Guid.NewGuid();
			_deliveryToEdit = new Delivery
			{
				Id = _deliveryToEditId,
				DeliveryNoteNumber = "DR2000",
				CreatedBy = _userContext.GetCurrentUser(),
				DateCreated = DateTime.Now,
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Customer Name" },
				Fao = "Graham Robertson"
			};
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
			Assert.That(_savedDelivery.DeliveryNoteNumber.StartsWith("DR"));
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

		[Test]
		public void Edit_ValidDeliveryDetails_ItemSuccessfullyEdited()
		{
			var fao = "g. robertson";
			var deliveryRepositoryMock = MockRepository.GenerateMock<IDeliveryRepository>();
			deliveryRepositoryMock.Stub(x => x.GetById(_deliveryToEditId)).Return(_deliveryToEdit);
			deliveryRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_deliveryService = DeliveryServiceFactory.Create(_userContext, deliveryRepositoryMock, MockRepository.GenerateStub<ICustomerRepository>());
			EditDelivery(_deliveryToEditId, fao);
			deliveryRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(fao, _deliveryToEdit.Fao);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidDeliveryId_ArgumentException()
		{
			var fao = "g. robertson";
			var deliveryRepositoryStub = MockRepository.GenerateMock<IDeliveryRepository>();
			deliveryRepositoryStub.Stub(x => x.GetById(_deliveryToEditId)).Return(null);
			_deliveryService = DeliveryServiceFactory.Create(_userContext, deliveryRepositoryStub, MockRepository.GenerateStub<ICustomerRepository>());
			EditDelivery(_deliveryToEditId, fao);
		}

		[Test]
		public void Edit_FaoGreaterThan255_DomainValidationException()
		{
			var fao = new string('A', 256);
			var deliveryRepositoryStub = MockRepository.GenerateMock<IDeliveryRepository>();
			deliveryRepositoryStub.Stub(x => x.GetById(_deliveryToEditId)).Return(_deliveryToEdit);
			_deliveryService = DeliveryServiceFactory.Create(_userContext, deliveryRepositoryStub, MockRepository.GenerateStub<ICustomerRepository>());
			EditDelivery(_deliveryToEditId, fao);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidFao));
		}

		private void EditDelivery(Guid deliveryId, string fao)
		{
			try
			{
				_deliveryToEdit = _deliveryService.Edit(deliveryId, fao);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}