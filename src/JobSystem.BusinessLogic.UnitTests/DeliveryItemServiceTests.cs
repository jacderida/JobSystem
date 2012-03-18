using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using JobSystem.TestHelpers;
using Rhino.Mocks;
using JobSystem.DataModel.Repositories;
using JobSystem.TestHelpers.RepositoryHelpers;
using JobSystem.Resources.Delivery;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class DeliveryItemServiceTests
	{
		private IUserContext _userContext;
		private DomainValidationException _domainValidationException;
		private DeliveryItemService _deliveryItemService;
		private PendingDeliveryItem _savedPendingItem;

		[SetUp]
		public void Setup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_domainValidationException = null;
			_savedPendingItem = null;
		}

		[Test]
		public void CreatePending_JobItemWithoutQuoteItem_PendingItemCreatedSuccessfully()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			var deliveryItemRepositoryMock = MockRepository.GenerateMock<IDeliveryItemRepository>();
			deliveryItemRepositoryMock.Expect(x => x.CreatePending(null)).IgnoreArguments();
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				deliveryItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsNull(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
			deliveryItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
			Assert.IsNotNull(_savedPendingItem.JobItem);
			Assert.IsNotNull(_savedPendingItem.Customer);
		}

		[Test]
		public void CreatePending_JobItemWithQuoteItem_PendingItemCreatedSuccessfully()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			var deliveryItemRepositoryMock = MockRepository.GenerateMock<IDeliveryItemRepository>();
			deliveryItemRepositoryMock.Expect(x => x.CreatePending(null)).IgnoreArguments();
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				deliveryItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
			deliveryItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
			Assert.IsNotNull(_savedPendingItem.JobItem);
			Assert.IsNotNull(_savedPendingItem.QuoteItem);
			Assert.IsNotNull(_savedPendingItem.Customer);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
		}

		[Test]
		public void CreatePending_JobItemAlreadyHasPendingItem_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsJobItemHasPendingQuoteItem_ReturnsTrue(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.PendingItemExists));
		}

		[Test]
		public void CreatePending_JobIsPending_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsNull(customerId));
			CreatePending(id, customerId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.JobPending));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidCustomerId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsNull(customerId));
			CreatePending(id, customerId, jobItemId, notes);
		}

		[Test]
		public void CreatePending_NotesGreaterThan255Characters_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = new string('a', 256);

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.InvalidNotes));
		}

		[Test]
		public void CreatePending_UserHasInsufficientSecurityClearance_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var notes = "some notes";

			_deliveryItemService = DeliveryItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IDeliveryRepository>(),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_GetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePending(id, customerId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.InsufficientSecurityClearance));
		}

		private void CreatePending(Guid id, Guid customerId, Guid jobItemId, string notes)
		{
			try
			{
				_savedPendingItem = _deliveryItemService.CreatePending(id, jobItemId, customerId, notes);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}