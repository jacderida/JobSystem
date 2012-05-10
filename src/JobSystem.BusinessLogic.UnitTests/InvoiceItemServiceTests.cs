using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Resources.Invoices;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class InvoiceItemServiceTests
	{
		private InvoiceItemService _invoiceItemService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private PendingInvoiceItem _savedPendingItem;
		private Guid _jobItemForCreatePendingId;
		private JobItem _jobItemForCreatePending;
		private QuoteItem _quoteItemForCreatePending;

		private Guid _jobItemForCreateFromPendingId;
		private JobItem _jobItemForCreateFromPending;
		private Guid _invoiceForCreateFromPendingId;
		private Invoice _invoiceForCreateFromPending;
		private InvoiceItem _savedInvoiceItemFromPending;

		[SetUp]
		public void Setup()
		{
			_savedPendingItem = null;
			_domainValidationException = null;
			_userContext =
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			var customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" };
			_jobItemForCreatePendingId = Guid.NewGuid();
			_jobItemForCreatePending = new JobItem
			{
				Id = _jobItemForCreatePendingId,
				ItemNo = 1,
				SerialNo = "12345",
				CalPeriod = 12,
				CreatedUser = _userContext.GetCurrentUser(),
				Created = DateTime.Now,
				Field = new ListItem { Id = Guid.NewGuid(), Name = "Electrical", Type = ListItemType.CategoryElectrical, Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Category", Type = ListItemCategoryType.JobItemCategory } },
				InitialStatus = new ListItem { Id = Guid.NewGuid(), Name = "House Calibration", Type = ListItemType.InitialStatusHouseCalibration, Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Initial Status", Type = ListItemCategoryType.JobItemInitialStatus } },
				Status = new ListItem { Id = Guid.NewGuid(), Name = "House Calibration", Type = ListItemType.InitialStatusHouseCalibration, Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Initial Status", Type = ListItemCategoryType.JobItemInitialStatus } },
				Instrument = new Instrument { Id = Guid.NewGuid(), Manufacturer = "Druck", ModelNo = "DPI601IS", Range = "Not specified", Description = "Digital Pressure Gauge" },
				Job = new Job { Id = Guid.NewGuid(), JobNo = "JR2000", IsPending = false, Customer = customer },
			};
			_quoteItemForCreatePending = new QuoteItem
			{
				Id = Guid.NewGuid(),
				ItemNo = 1,
				Calibration = 20,
				Labour = 25,
				Carriage = 65,
				Investigation = 75,
				Parts = 55,
				JobItem = _jobItemForCreatePending,
				Quote = new Quote { Id = Guid.NewGuid(), QuoteNumber = "QR2000", CreatedBy = _userContext.GetCurrentUser(), Customer = customer, OrderNumber = "ORD12345" },
				DateAccepted = DateTime.Now,
				Report = "item passed cal",
				Status = new ListItem { Id = Guid.NewGuid(), Name = "Quote Prepared", Type = ListItemType.StatusQuoteAccepted, Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.JobItemStatus } },
				Days = 20
			};
			_jobItemForCreateFromPendingId = Guid.NewGuid();
			_jobItemForCreateFromPending = new JobItem
			{
				Id = _jobItemForCreateFromPendingId,
				ItemNo = 1,
				SerialNo = "12345",
				CreatedUser = _userContext.GetCurrentUser(),
				Created = DateTime.Now,
			};
			_invoiceForCreateFromPendingId = Guid.NewGuid();
			_invoiceForCreateFromPending = new Invoice
			{
				Id = _invoiceForCreateFromPendingId,
				InvoiceNumber = "IR2000",
				Customer = customer,
				DateCreated = DateTime.Now,
			};
		}

		#region CreatePending

		[Test]
		public void CreatePending_ValidDetailsOrderOnQuote_ItemCreatedWithQuoteOrderNo()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;

			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(false);
			invoiceItemRepositoryMock.Expect(x => x.CreatePendingItem(null)).IgnoreArguments();
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				CompanyDetailsRepositoryTestHelper.GetCompanyDetailsRepository_StubsApplyAllPrices_ReturnsFalse(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			invoiceItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
			Assert.IsNotNull(_savedPendingItem.JobItem);
			Assert.AreEqual(_quoteItemForCreatePending.JobItem.Instrument.ToString(), _savedPendingItem.Description);
			Assert.AreEqual(_quoteItemForCreatePending.Quote.OrderNumber, _savedPendingItem.OrderNo);
			Assert.AreEqual(_quoteItemForCreatePending.Calibration, _savedPendingItem.CalibrationPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Labour, _savedPendingItem.RepairPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Parts, _savedPendingItem.PartsPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Carriage, _savedPendingItem.CarriagePrice);
			Assert.AreEqual(0, _savedPendingItem.InvestigationPrice);
		}

		[Test]
		public void CreatePending_ValidDetailsOrderOnJob_ItemCreatedWithQuoteOrderNo()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;
			_quoteItemForCreatePending.Quote.OrderNumber = null;
			_jobItemForCreatePending.Job.OrderNo = "OR1234";

			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(false);
			invoiceItemRepositoryMock.Expect(x => x.CreatePendingItem(null)).IgnoreArguments();
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				CompanyDetailsRepositoryTestHelper.GetCompanyDetailsRepository_StubsApplyAllPrices_ReturnsFalse(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			invoiceItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
			Assert.IsNotNull(_savedPendingItem.JobItem);
			Assert.AreEqual(_quoteItemForCreatePending.JobItem.Instrument.ToString(), _savedPendingItem.Description);
			Assert.AreEqual(_jobItemForCreatePending.Job.OrderNo, _savedPendingItem.OrderNo);
			Assert.AreEqual(_quoteItemForCreatePending.Calibration, _savedPendingItem.CalibrationPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Labour, _savedPendingItem.RepairPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Parts, _savedPendingItem.PartsPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Carriage, _savedPendingItem.CarriagePrice);
			Assert.AreEqual(0, _savedPendingItem.InvestigationPrice);
		}

		[Test]
		public void CreatePending_QuoteItemRejected_ItemCreatedWithQuoteOrderNo()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;
			_quoteItemForCreatePending.Status =
				new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Quote Rejected",
					Type = ListItemType.StatusQuoteRejected,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Status", Type = ListItemCategoryType.JobItemStatus }
				};

			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(false);
			invoiceItemRepositoryMock.Expect(x => x.CreatePendingItem(null)).IgnoreArguments();
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				CompanyDetailsRepositoryTestHelper.GetCompanyDetailsRepository_StubsApplyAllPrices_ReturnsFalse(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			invoiceItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
			Assert.IsNotNull(_savedPendingItem.JobItem);
			Assert.AreEqual(_quoteItemForCreatePending.JobItem.Instrument.ToString(), _savedPendingItem.Description);
			Assert.AreEqual(_quoteItemForCreatePending.Quote.OrderNumber, _savedPendingItem.OrderNo);
			Assert.AreEqual(0, _savedPendingItem.CalibrationPrice);
			Assert.AreEqual(0, _savedPendingItem.RepairPrice);
			Assert.AreEqual(0, _savedPendingItem.PartsPrice);
			Assert.AreEqual(0, _savedPendingItem.CarriagePrice);
			Assert.AreEqual(_quoteItemForCreatePending.Investigation, _savedPendingItem.InvestigationPrice);
		}

		[Test]
		public void CreatePending_ApplyAllPrices_ItemCreatedWithAllPricesApplied()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;

			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Stub(x => x.JobItemHasPendingInvoiceItem(jobItemId)).Return(false);
			invoiceItemRepositoryMock.Expect(x => x.CreatePendingItem(null)).IgnoreArguments();
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				CompanyDetailsRepositoryTestHelper.GetCompanyDetailsRepository_StubsApplyAllPrices_ReturnsTrue(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			invoiceItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
			Assert.IsNotNull(_savedPendingItem.JobItem);
			Assert.AreEqual(_quoteItemForCreatePending.JobItem.Instrument.ToString(), _savedPendingItem.Description);
			Assert.AreEqual(_quoteItemForCreatePending.Quote.OrderNumber, _savedPendingItem.OrderNo);
			Assert.AreEqual(_quoteItemForCreatePending.Calibration, _savedPendingItem.CalibrationPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Labour, _savedPendingItem.RepairPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Parts, _savedPendingItem.PartsPrice);
			Assert.AreEqual(_quoteItemForCreatePending.Carriage, _savedPendingItem.CarriagePrice);
			Assert.AreEqual(_quoteItemForCreatePending.Investigation, _savedPendingItem.InvestigationPrice);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_IdNotSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var jobItemId = _jobItemForCreatePendingId;

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
		}

		[Test]
		public void CreatePending_QuoteItemPrepared_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;
			_quoteItemForCreatePending.Status =
				new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Quote Prepared",
					Type = ListItemType.StatusQuotedPrepared,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Status", Type = ListItemCategoryType.JobItemStatus }
				};

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.QuoteStatusInvalid));
		}

		[Test]
		public void CreatePending_QuoteItemNotRaised_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsNull(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.QuoteItemNull));
		}

		[Test]
		public void CreatePending_JobIsPending_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;
			_jobItemForCreatePending.Job.IsPending = true;

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.JobPending));
		}

		[Test]
		public void CreatePending_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		[Test]
		public void CreatePending_JobItemAlreadyHasPendingItem_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = _jobItemForCreatePendingId;

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				InvoiceItemRepositoryTestHelper.GetInvoiceItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsTrue(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending),
				MockRepository.GenerateStub<IListItemRepository>());
			CreatePending(id, jobItemId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.JobItemHasPendingItem));
		}

		private void CreatePending(Guid id, Guid jobItemId)
		{
			try
			{
				_savedPendingItem = _invoiceItemService.CreatePending(id, jobItemId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region CreateFromPending

		[Test]
		public void CreateFromPending_ValidDetailsWhenInvoiceHasZeroItems_InvoiceItemSuccessfullyCreated()
		{
			var id = Guid.NewGuid();
			var invoiceId = _invoiceForCreateFromPendingId;
			var description = "Druck, DPI601IS, Digital Pressure Indicator";
			var calibrationPrice = 25;
			var repairPrice = 38;
			var partsPrice = 200;
			var carriagePrice = 30;
			var investigationPrice = 0;

			var invoiceRepositoryStub = MockRepository.GenerateStub<IInvoiceRepository>();
			invoiceRepositoryStub.Stub(x => x.GetInvoiceItemCount(invoiceId)).Return(0);
			invoiceRepositoryStub.Stub(x => x.GetById(invoiceId)).Return(_invoiceForCreateFromPending);
			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemForCreateFromPendingId, 0, 0, "Item invoiced on IR2000", ListItemType.StatusInvoiced, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemForCreateFromPending)).IgnoreArguments();
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusInvoiced)).Return(new ListItem { Id = Guid.NewGuid(), Name = "Invoiced", Type = ListItemType.StatusInvoiced });

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				invoiceRepositoryStub,
				invoiceItemRepositoryMock,
				jobItemRepositoryMock,
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				listItemRepositoryStub);
			CreateFromPending(id, invoiceId, description, calibrationPrice, repairPrice, partsPrice, carriagePrice, investigationPrice, _jobItemForCreateFromPending);
			invoiceItemRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedInvoiceItemFromPending.Id);
			Assert.AreEqual(1, _savedInvoiceItemFromPending.ItemNo);
			Assert.AreEqual(description, _savedInvoiceItemFromPending.Description);
			Assert.AreEqual(calibrationPrice, _savedInvoiceItemFromPending.CalibrationPrice);
			Assert.AreEqual(repairPrice, _savedInvoiceItemFromPending.RepairPrice);
			Assert.AreEqual(partsPrice, _savedInvoiceItemFromPending.PartsPrice);
			Assert.AreEqual(carriagePrice, _savedInvoiceItemFromPending.CarriagePrice);
			Assert.AreEqual(investigationPrice, _savedInvoiceItemFromPending.InvestigationPrice);
			Assert.IsNotNull(_savedInvoiceItemFromPending.Invoice);
			Assert.IsNotNull(_savedInvoiceItemFromPending.JobItem);
			Assert.AreEqual(ListItemType.StatusInvoiced, _jobItemForCreateFromPending.Status.Type);
		}

		[Test]
		public void CreateFromPending_ValidDetailsWhenInvoiceHasOneItem_InvoiceItemSuccessfullyCreated()
		{
			var id = Guid.NewGuid();
			var invoiceId = _invoiceForCreateFromPendingId;
			var description = "Druck, DPI601IS, Digital Pressure Indicator";
			var calibrationPrice = 25;
			var repairPrice = 38;
			var partsPrice = 200;
			var carriagePrice = 30;
			var investigationPrice = 0;

			var invoiceRepositoryStub = MockRepository.GenerateStub<IInvoiceRepository>();
			invoiceRepositoryStub.Stub(x => x.GetInvoiceItemCount(invoiceId)).Return(1);
			invoiceRepositoryStub.Stub(x => x.GetById(invoiceId)).Return(_invoiceForCreateFromPending);
			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemForCreateFromPendingId, 0, 0, "Item invoiced on IR2000", ListItemType.StatusInvoiced, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemForCreateFromPending)).IgnoreArguments();
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusInvoiced)).Return(new ListItem { Id = Guid.NewGuid(), Name = "Invoiced", Type = ListItemType.StatusInvoiced });

			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				invoiceRepositoryStub,
				invoiceItemRepositoryMock,
				jobItemRepositoryMock,
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				listItemRepositoryStub);
			CreateFromPending(id, invoiceId, description, calibrationPrice, repairPrice, partsPrice, carriagePrice, investigationPrice, _jobItemForCreateFromPending);
			invoiceItemRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedInvoiceItemFromPending.Id);
			Assert.AreEqual(2, _savedInvoiceItemFromPending.ItemNo);
			Assert.AreEqual(description, _savedInvoiceItemFromPending.Description);
			Assert.AreEqual(calibrationPrice, _savedInvoiceItemFromPending.CalibrationPrice);
			Assert.AreEqual(repairPrice, _savedInvoiceItemFromPending.RepairPrice);
			Assert.AreEqual(partsPrice, _savedInvoiceItemFromPending.PartsPrice);
			Assert.AreEqual(carriagePrice, _savedInvoiceItemFromPending.CarriagePrice);
			Assert.AreEqual(investigationPrice, _savedInvoiceItemFromPending.InvestigationPrice);
			Assert.IsNotNull(_savedInvoiceItemFromPending.Invoice);
			Assert.IsNotNull(_savedInvoiceItemFromPending.JobItem);
			Assert.AreEqual(ListItemType.StatusInvoiced, _jobItemForCreateFromPending.Status.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreateFromPending_InvalidInvoiceId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var invoiceId = Guid.NewGuid();
			var description = "Druck, DPI601IS, Digital Pressure Indicator";
			var calibrationPrice = 25;
			var repairPrice = 38;
			var partsPrice = 200;
			var carriagePrice = 30;
			var investigationPrice = 0;

			var invoiceItemRepositoryMock = MockRepository.GenerateMock<IInvoiceItemRepository>();
			invoiceItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				InvoiceRepositoryTestHelper.GetInvoiceRepository_StubsGetById_ReturnsNull(invoiceId),
				invoiceItemRepositoryMock,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			CreateFromPending(id, invoiceId, description, calibrationPrice, repairPrice, partsPrice, carriagePrice, investigationPrice, _jobItemForCreateFromPending);
		}

		private void CreateFromPending(
			Guid id, Guid invoiceId, string description, decimal calibrationPrice, decimal repairPrice, decimal partsPrice, decimal carriagePrice, decimal investigationPrice, JobItem jobItem)
		{
			try
			{
				_savedInvoiceItemFromPending = _invoiceItemService.CreateFromPending(
					id, invoiceId, description, calibrationPrice, repairPrice, partsPrice, carriagePrice, investigationPrice, jobItem);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetPendingInvoiceItems

		[Test]
		public void GetPendingInvoiceItems_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				MockRepository.GenerateStub<IInvoiceItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			GetPendingInvoiceItems();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void GetPendingInvoiceItems()
		{
			try
			{
				_invoiceItemService.GetPendingInvoiceItems();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetInvoiceItems

		[Test]
		public void GetInvoiceItems_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_invoiceItemService = InvoiceItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<ICompanyDetailsRepository>(),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				MockRepository.GenerateStub<IInvoiceItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>());
			GetInvoiceItems();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void GetInvoiceItems()
		{
			try
			{
				_invoiceItemService.GetInvoiceItems(Guid.NewGuid());
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}