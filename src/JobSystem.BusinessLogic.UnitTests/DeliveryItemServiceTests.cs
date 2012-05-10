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
		private JobItemService _jobItemService;
		private DeliveryItemService _deliveryItemService;
		private DeliveryItem _savedDeliveryItem;
		private PendingDeliveryItem _savedPendingItem;
		private Guid _jobItemToUpdateId;
		private JobItem _jobItemToUpdate;
		private DeliveryItem _deliveryItemForEdit;
		private Guid _deliveryItemForEditId;

		[SetUp]
		public void Setup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_domainValidationException = null;
			_savedPendingItem = null;
			_savedDeliveryItem = null;
			_jobItemService = null;
			_deliveryItemService = null;
			_jobItemToUpdateId = Guid.NewGuid();
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
			_deliveryItemForEditId = Guid.NewGuid();
			_deliveryItemForEdit = new DeliveryItem
			{
				Id = _deliveryItemForEditId,
				Delivery = new Delivery { Id = Guid.NewGuid(), DeliveryNoteNumber = "DR2000", Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael" } },
				ItemNo = 1,
			};
		}

		#region Create

		[Test]
		public void Create_DeliveryWith0ItemsAndJobItemWithQuoteItem_DeliveryItemCreated()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			var deliveryRepositoryMock = MockRepository.GenerateMock<IDeliveryItemRepository>();
			deliveryRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item added to delivery note DR2000", ListItemType.StatusDeliveryNoteProduced, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith0Items_ReturnsDelivery(deliveryId),
				deliveryRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
			deliveryRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedDeliveryItem.Id);
			Assert.AreEqual(1, _savedDeliveryItem.ItemNo);
			Assert.IsNotNull(_savedDeliveryItem.Delivery);
			Assert.IsNotNull(_savedDeliveryItem.JobItem);
			Assert.IsNotNull(_savedDeliveryItem.QuoteItem);
			Assert.AreEqual(ListItemType.StatusDeliveryNoteProduced, _jobItemToUpdate.Status.Type);
		}

		[Test]
		public void Create_DeliveryWith1ItemsAndJobItemWithQuoteItem_DeliveryItemCreated()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			var deliveryRepositoryMock = MockRepository.GenerateMock<IDeliveryItemRepository>();
			deliveryRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item added to delivery note DR2000", ListItemType.StatusDeliveryNoteProduced, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				deliveryRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
			deliveryRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedDeliveryItem.Id);
			Assert.AreEqual(2, _savedDeliveryItem.ItemNo);
			Assert.IsNotNull(_savedDeliveryItem.Delivery);
			Assert.IsNotNull(_savedDeliveryItem.JobItem);
			Assert.IsNotNull(_savedDeliveryItem.QuoteItem);
			Assert.AreEqual(ListItemType.StatusDeliveryNoteProduced, _jobItemToUpdate.Status.Type);
		}

		[Test]
		public void Create_DeliveryWith0ItemsAndJobItemWithNoQuoteItem_DeliveryItemCreated()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			var deliveryRepositoryMock = MockRepository.GenerateMock<IDeliveryItemRepository>();
			deliveryRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item added to delivery note DR2000", ListItemType.StatusDeliveryNoteProduced, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				deliveryRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsNull(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
			deliveryRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedDeliveryItem.Id);
			Assert.AreEqual(2, _savedDeliveryItem.ItemNo);
			Assert.IsNotNull(_savedDeliveryItem.Delivery);
			Assert.IsNotNull(_savedDeliveryItem.JobItem);
			Assert.IsNull(_savedDeliveryItem.QuoteItem);
			Assert.AreEqual(ListItemType.StatusDeliveryNoteProduced, _jobItemToUpdate.Status.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
		}

		[Test]
		public void Create_JobIsPending_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.JobPending));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidDeliveryId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetById_ReturnsNull(deliveryId),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
		}

		[Test]
		public void Create_NotesGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = new string('a', 256);

			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				_userContext,
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.InvalidNotes));
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var deliveryId = Guid.NewGuid();
			var jobItemId = _jobItemToUpdateId;
			var notes = "some notes";

			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			_deliveryItemService = DeliveryItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				DeliveryRepositoryTestHelper.GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(deliveryId),
				MockRepository.GenerateStub<IDeliveryItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetByType_ReturnsListItem(new ListItemType[] { ListItemType.StatusDeliveryNoteProduced }),
				MockRepository.GenerateStub<ICustomerRepository>());
			CreateDeliveryItem(id, deliveryId, jobItemId, notes);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(DeliveryItemMessages.InsufficientSecurityClearance));
		}

		private void CreateDeliveryItem(Guid id, Guid deliveryId, Guid jobItemId, string notes)
		{
			try
			{
				_savedDeliveryItem = _deliveryItemService.Create(id, deliveryId, jobItemId, notes);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region CreatePending

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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsNull(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId),
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

		#endregion
		#region Edit



		private void Edit(Guid deliveryItemId, string notes)
		{
			try
			{
				_deliveryItemForEdit = _deliveryItemService.Edit(deliveryItemId, notes);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}