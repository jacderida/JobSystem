using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.RepositoryHelpers;
using JobSystem.Resources.Invoices;

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
				Location = new ListItem { Id = Guid.NewGuid(), Name = "House Calibration", Type = ListItemType.InitialWorkLocationHouseCalibration, Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Location", Type = ListItemCategoryType.JobItemLocation } },
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
		}

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
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				invoiceItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsNull(jobItemId));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsFalse(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
				InvoiceItemRepositoryTestHelper.GetJobItemRepository_StubsJobItemHasPendingInvoiceItem_ReturnsTrue(jobItemId),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(_jobItemForCreatePending),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(jobItemId, _quoteItemForCreatePending));
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
	}
}