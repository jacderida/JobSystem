using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.QuoteItems;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class QuoteItemServiceTests
	{
		private QuoteItem _savedQuoteItem;
		private QuoteItemService _quoteItemService;
		private JobItemService _jobItemService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);
		private Guid _jobItemToUpdateId;
		private JobItem _jobItemToUpdate;
		private PendingQuoteItem _savedPendingItem;
		private Guid _pendingItemForEditId;
		private PendingQuoteItem _pendingItemForEdit;
		private Quote _quoteForEdit;
		private Guid _quoteForEditId;
		private QuoteItem _quoteItemForEdit;
		private Guid _quoteItemForEditId;

		private Guid _quoteItemForAcceptId;
		private QuoteItem _quoteItemForAccept;

		private Guid _quoteItemForRejectId;
		private QuoteItem _quoteItemForReject;

		[SetUp]
		public void Setup()
		{
			_jobItemToUpdateId = Guid.NewGuid();
			_quoteItemService = null;
			_savedQuoteItem = null;
			_savedPendingItem = null;
			_domainValidationException = null;
			_pendingItemForEditId = Guid.NewGuid();
			AppDateTime.GetUtcNow = () => _dateCreated;
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
				CreatedUser = _userContext.GetCurrentUser(),
			};
			_pendingItemForEdit = new PendingQuoteItem
			{
				Id = _pendingItemForEditId,
				JobItem = new JobItem { Id = Guid.NewGuid() },
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				Calibration = 30,
				Labour = 40,
				Carriage = 30,
				Parts = 10,
				Investigation = 40,
				Days = 30,
				Report = "item calibrated ok",
				BeyondEconomicRepair = false
			};
			_quoteForEditId = Guid.NewGuid();
			_quoteForEdit = new Quote
			{
				Id = _quoteForEditId,
				CreatedBy = _userContext.GetCurrentUser(),
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				DateCreated = DateTime.Now,
				OrderNumber = "PO1000",
				AdviceNumber = "AD1000",
				QuoteNumber = "QR2000",
				Revision = 1
			};
			_quoteItemForEditId = Guid.NewGuid();
			_quoteItemForEdit = new QuoteItem
			{
				Id = _quoteItemForEditId,
				ItemNo = 1,
				Labour = 45,
				Calibration = 55,
				Parts = 23,
				Carriage = 14,
				Investigation = 45,
				Days = 30,
				Status = new ListItem
				{
					Id = Guid.NewGuid(),
					Category = new ListItemCategory
					{
						Id = Guid.NewGuid(),
						Name = "Job Item Status",
						Type = ListItemCategoryType.JobItemStatus
					},
					Name = "Quote Prepared",
					Type = ListItemType.StatusQuotedPrepared
				},
				Quote = new Quote { Id = _quoteForEditId }
			};

			_quoteItemForAcceptId = Guid.NewGuid();
			_quoteItemForAccept = new QuoteItem
			{
				Id = _quoteItemForAcceptId,
				ItemNo = 1,
				Labour = 45,
				Calibration = 55,
				Parts = 23,
				Carriage = 14,
				Investigation = 45,
				Days = 30,
				Status = new ListItem
				{
					Id = Guid.NewGuid(),
					Category = new ListItemCategory
					{
						Id = Guid.NewGuid(),
						Name = "Job Item Status",
						Type = ListItemCategoryType.JobItemStatus
					},
					Name = "Quote Prepared",
					Type = ListItemType.StatusQuotedPrepared
				},
				Quote = new Quote { Id = _quoteForEditId, QuoteNumber = "QR2000" },
				JobItem = _jobItemToUpdate
			};

			_quoteItemForRejectId = Guid.NewGuid();
			_quoteItemForReject = new QuoteItem
			{
				Id = _quoteItemForAcceptId,
				ItemNo = 1,
				Labour = 45,
				Calibration = 55,
				Parts = 23,
				Carriage = 14,
				Investigation = 45,
				Days = 30,
				Status = new ListItem
				{
					Id = Guid.NewGuid(),
					Category = new ListItemCategory
					{
						Id = Guid.NewGuid(),
						Name = "Job Item Status",
						Type = ListItemCategoryType.JobItemStatus
					},
					Name = "Quote Prepared",
					Type = ListItemType.StatusQuotedPrepared
				},
				Quote = new Quote { Id = _quoteForEditId, QuoteNumber = "QR2000" },
				JobItem = _jobItemToUpdate
			};
		}

		#region Create

		[Test]
		public void Create_ValidQuoteItemDetailsOnQuoteWith0Items_QuoteItemCreated()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;

			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item quoted on QR2000", ListItemType.StatusQuotedPrepared, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				quoteItemRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			CreateQuoteItem(id, quoteId, _jobItemToUpdateId, labour, calibration, parts, carriage, investigation, report, days, ber);
			jobItemRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedQuoteItem.Id);
			Assert.AreEqual(1, _savedQuoteItem.ItemNo);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _savedQuoteItem.Status.Type);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _jobItemToUpdate.Status.Type);
		}

		[Test]
		public void Create_ValidQuoteItemDetailsOnQuoteWith1Item_QuoteItemCreated()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;

			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item quoted on QR2000", ListItemType.StatusQuotedPrepared, ListItemType.WorkTypeAdministration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith1Item(quoteId),
				quoteItemRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			CreateQuoteItem(id, quoteId, _jobItemToUpdateId, labour, calibration, parts, carriage, investigation, report, days, ber);
			jobItemRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedQuoteItem.Id);
			Assert.AreEqual(2, _savedQuoteItem.ItemNo);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _savedQuoteItem.Status.Type);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _jobItemToUpdate.Status.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, _jobItemToUpdateId, labour, calibration, parts, carriage, investigation, report, days, ber);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidQuoteIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsNull(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, _jobItemToUpdateId, labour, calibration, parts, carriage, investigation, report, days, ber);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidJobIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
		}

		[Test]
		public void Create_JobIsPending_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.PendingJob));
		}

		[Test]
		public void Create_LabourLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = -50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidLabour));
		}

		[Test]
		public void Create_CalibrationLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = -50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCalibration));
		}

		[Test]
		public void Create_PartsLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = -20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidParts));
		}

		[Test]
		public void Create_CarriageLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = -20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCarriage));
		}

		[Test]
		public void Create_InvestigationLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = -30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidInvestigation));
		}

		[Test]
		public void Create_DaysLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = -30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidDays));
		}

		[Test]
		public void Create_ReportGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = new string('a', 2001);
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidReport));
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteId = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "calibrated successfully";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = JobItemServiceFactory.Create(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
			CreateQuoteItem(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void CreateQuoteItem(Guid id, Guid quoteId, Guid jobItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool ber)
		{
			try
			{
				_savedQuoteItem = _quoteItemService.Create(id, quoteId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber);
				_jobItemToUpdate = _jobItemService.GetById(_jobItemToUpdateId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region CreatePending

		[Test]
		public void CreatePending_ValidPendingDetails_PendingItemCreated()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Expect(x => x.CreatePendingQuoteItem(null)).IgnoreArguments();
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidId_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
		}

		[Test]
		public void CreatePending_JobItemHasPendingItem_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.JobItemHasPendingQuoteItem(jobItemId)).Return(true);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.PendingItemExists));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
		}

		[Test]
		public void CreatePending_InvalidJobItemId_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.PendingJob));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidCustomerId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsNull(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
		}

		[Test]
		public void CreatePending_LabourLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = -50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidLabour));
		}

		[Test]
		public void CreatePending_CalibrationLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = -50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCalibration));
		}

		[Test]
		public void CreatePending_PartsLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = -20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidParts));
		}

		[Test]
		public void CreatePending_CarriageLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = -20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCarriage));
		}

		[Test]
		public void CreatePending_InvestigationLessThan0_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = -30m;
			var report = "Item calibrated ok";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidInvestigation));
		}

		[Test]
		public void CreatePending_ReportGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = new string('a', 2001);
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidReport));
		}

		[Test]
		public void CreatePending_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "a report";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		[Test]
		public void CreatePending_OrderNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "a report";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = new string('a', 51);
			var adviceNo = "AD1000";

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidOrderNo));
		}

		[Test]
		public void CreatePending_AdviceNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "a report";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = new string('a', 51);

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			CreatePendingQuoteItem(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidAdviceNo));
		}

		private void CreatePendingQuoteItem(
			Guid id, Guid customerId, Guid jobItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool ber, string orderNo, string adviceNo)
		{
			try
			{
				_savedPendingItem = _quoteItemService.CreatePending(id, customerId, jobItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region EditPending

		[Test]
		public void EditPending_ValidDetails_ItemEdited()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Expect(x => x.UpdatePendingItem(null)).IgnoreArguments();
			quoteItemRepositoryMock.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryMock,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(150, _pendingItemForEdit.Labour);
			Assert.AreEqual(50, _pendingItemForEdit.Calibration);
			Assert.AreEqual(20, _pendingItemForEdit.Parts);
			Assert.AreEqual(20, _pendingItemForEdit.Carriage);
			Assert.AreEqual(30, _pendingItemForEdit.Investigation);
			Assert.AreEqual("Item calibrated ok", _pendingItemForEdit.Report);
			Assert.AreEqual(true, _pendingItemForEdit.BeyondEconomicRepair);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EditPending_InvalidPendingItemId_ArgumentExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(null);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
		}

		[Test]
		public void EditPending_LabourLessThan0_DomainValidationExceptionThrown()
		{
			var labour = -150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidLabour));
		}

		[Test]
		public void EditPending_CalibrationLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = -50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCalibration));
		}

		[Test]
		public void EditPending_PartsLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = -20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidParts));
		}

		[Test]
		public void EditPending_CarriageLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = -20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCarriage));
		}

		[Test]
		public void EditPending_InvestigationLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = -30m;
			var report = "Item calibrated ok";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidInvestigation));
		}

		[Test]
		public void EditPending_DaysLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "Item calibrated ok";
			var days = -20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidDays));
		}

		[Test]
		public void EditPending_ReportGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = new string('a', 2001);
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidReport));
		}

		[Test]
		public void EditPending_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var labour = 150m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "report";
			var days = 20;
			var ber = true;
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		[Test]
		public void EditPending_OrderNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "a report";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = new string('a', 51);
			var adviceNo = "AD1000";

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidOrderNo));
		}

		[Test]
		public void EditPending_AdviceNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var labour = 50m;
			var calibration = 50m;
			var parts = 20m;
			var carriage = 20m;
			var investigation = 30m;
			var report = "a report";
			var days = 30;
			var ber = false;
			var jobItemId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = new string('a', 51);

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetPendingQuoteItem(_pendingItemForEditId)).Return(_pendingItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				MockRepository.GenerateStub<IListItemRepository>(),
				QuoteItemServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId));
			EditPendingItem(_pendingItemForEditId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidAdviceNo));
		}

		private void EditPendingItem(Guid pendingItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool ber, string orderNo, string adviceNo)
		{
			try
			{
				_pendingItemForEdit = _quoteItemService.EditPending(pendingItemId, labour, calibration, parts, carriage, investigation, report, days, ber, orderNo, adviceNo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Edit

		[Test]
		public void Edit_ValidDetails_ItemEditedSuccessfully()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteRepositoryMock = MockRepository.GenerateMock<IQuoteRepository>();
			quoteRepositoryMock.Stub(x => x.GetById(_quoteForEditId)).Return(_quoteForEdit);
			quoteRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			quoteItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				quoteRepositoryMock,
				quoteItemRepositoryMock,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			quoteRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(2, _quoteForEdit.Revision);
			Assert.AreEqual(20, _quoteItemForEdit.Labour);
			Assert.AreEqual(30, _quoteItemForEdit.Calibration);
			Assert.AreEqual(24, _quoteItemForEdit.Parts);
			Assert.AreEqual(25, _quoteItemForEdit.Investigation);
			Assert.AreEqual("new report", _quoteItemForEdit.Report);
			Assert.AreEqual(20, _quoteItemForEdit.Days);
			Assert.AreEqual(true, _quoteItemForEdit.BeyondEconomicRepair);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidQuoteItemId_ArgumentExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(null);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
		}

		[Test]
		public void Edit_UserHasInsufficientSecurity_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(null);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		[Test]
		public void Edit_LabourLessThan0_DomainValidationExceptionThrown()
		{
			var labour = -20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidLabour));
		}

		[Test]
		public void Edit_CalibrationLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = -30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCalibration));
		}

		[Test]
		public void Edit_CarriageLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = -30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidCarriage));
		}

		[Test]
		public void Edit_PartsLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = -24;
			var investgation = 25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidParts));
		}

		[Test]
		public void Edit_InvestigationLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = -25;
			var report = "new report";
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidInvestigation));
		}

		[Test]
		public void Edit_DaysLessThan0_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = "new report";
			var days = -20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidDays));
		}

		[Test]
		public void Edit_ReportGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var labour = 20;
			var calibration = 30;
			var carriage = 30;
			var parts = 24;
			var investgation = 25;
			var report = new string('a', 2001);
			var days = 20;
			var ber = true;

			var quoteItemRepositoryStub = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(_quoteItemForEditId)).Return(_quoteItemForEdit);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Edit(_quoteItemForEditId, labour, calibration, parts, carriage, investgation, report, days, ber);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidReport));
		}

		private void Edit(Guid quoteItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool beyondEconomicRepair)
		{
			try
			{
				_quoteItemForEdit = _quoteItemService.Edit(quoteItemId, labour, calibration, parts, carriage, investigation, report, days, beyondEconomicRepair);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetPendingItems

		[Test]
		public void GetPendingItems_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetPendingItems();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void GetPendingItems()
		{
			try
			{
				_quoteItemService.GetPendingQuoteItems();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		[Test]
		public void GetPendingItemsWithIds_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetPendingItems(new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void GetPendingItems(List<Guid> pendingIds)
		{
			try
			{
				_quoteItemService.GetPendingQuoteItems(pendingIds);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetPendingQuoteItem

		[Test]
		public void GetPendingQuoteItem_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetPendingItem(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void GetPendingItem(Guid id)
		{
			try
			{
				_quoteItemService.GetPendingQuoteItemForJobItem(id);
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
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetById(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void GetById_InvalidQuoteItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var quoteItemRepositoryStub = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepositoryStub.Stub(x => x.GetById(id)).Return(null);
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryStub,
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetById(id);
		}

		private void GetById(Guid id)
		{
			try
			{
				_quoteItemService.GetById(id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetQuoteItemForJobItem

		[Test]
		public void GetQuoteItemForJobItem_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetQuoteItemForJobItem(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void GetQuoteItemForJobItem(Guid jobItemId)
		{
			try
			{
				_quoteItemService.GetQuoteItemForJobItem(jobItemId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetQuoteItems

		[Test]
		public void GetQuoteItems_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			GetQuoteItems(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void GetQuoteItems(Guid quoteId)
		{
			try
			{
				_quoteItemService.GetQuoteItems(quoteId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Accept

		[Test]
		public void Accept_ValidItemDetails_ItemAccepted()
		{
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item accepted on quote QR2000", ListItemType.StatusQuoteAccepted, ListItemType.WorkTypeAdministration));
			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Stub(x => x.GetById(_quoteItemForAcceptId)).Return(_quoteItemForAccept);
			quoteItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusQuoteAccepted)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Quote Accepted",
					Type = ListItemType.StatusQuoteAccepted,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Status", Type = ListItemCategoryType.JobItemStatus }
				});

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryMock,
				jobItemRepositoryMock,
				listItemRepositoryStub,
				MockRepository.GenerateStub<ICustomerRepository>());
			Accept(_quoteItemForAcceptId);
			jobItemRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(ListItemType.StatusQuoteAccepted, _quoteItemForAccept.Status.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Accept_InvalidQuoteItemId_ArgumentException()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetById_ReturnsNull(_quoteItemForAcceptId),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Accept(_quoteItemForAcceptId);
		}

		[Test]
		public void Accept_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Accept(_quoteItemForAcceptId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void Accept(Guid quoteItemId)
		{
			try
			{
				_quoteItemForAccept = _quoteItemService.Accept(quoteItemId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Reject

		[Test]
		public void Reject_ValidItemDetails_ItemAccepted()
		{
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item rejected on quote QR2000", ListItemType.StatusQuoteRejected, ListItemType.WorkTypeAdministration));
			var quoteItemRepositoryMock = MockRepository.GenerateMock<IQuoteItemRepository>();
			quoteItemRepositoryMock.Stub(x => x.GetById(_quoteItemForRejectId)).Return(_quoteItemForReject);
			quoteItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusQuoteRejected)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Name = "Quote Rejected",
					Type = ListItemType.StatusQuoteRejected,
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Status", Type = ListItemCategoryType.JobItemStatus }
				});

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				quoteItemRepositoryMock,
				jobItemRepositoryMock,
				listItemRepositoryStub,
				MockRepository.GenerateStub<ICustomerRepository>());
			Reject(_quoteItemForRejectId);
			jobItemRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(ListItemType.StatusQuoteRejected, _quoteItemForReject.Status.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Reject_InvalidQuoteItemId_ArgumentException()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteItemRepositoryTestHelper.GetQuoteItemRepository_StubsGetById_ReturnsNull(_quoteItemForRejectId),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Reject(_quoteItemForRejectId);
		}

		[Test]
		public void Reject_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member),
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				MockRepository.GenerateStub<IJobItemRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>());
			Reject(_quoteItemForRejectId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurity));
		}

		private void Reject(Guid quoteItemId)
		{
			try
			{
				_quoteItemForReject = _quoteItemService.Reject(quoteItemId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}