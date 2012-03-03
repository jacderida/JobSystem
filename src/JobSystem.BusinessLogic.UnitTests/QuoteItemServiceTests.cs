using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using NUnit.Framework;
using JobSystem.Framework;
using JobSystem.TestHelpers.Context;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.TestHelpers;
using JobSystem.Resources.QuoteItems;

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

		[SetUp]
		public void Setup()
		{
			_quoteItemService = null;
			_savedQuoteItem = null;
			_domainValidationException = null;
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
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item quoted on QR2000", ListItemType.StatusQuotedPrepared, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationQuoted));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				quoteItemRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, jobItemRepositoryMock);
			CreateQuoteItem(id, quoteId, _jobItemToUpdateId, labour, calibration, parts, carriage, investigation, report, days, ber);
			jobItemRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedQuoteItem.Id);
			Assert.AreEqual(1, _savedQuoteItem.ItemNo);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _savedQuoteItem.Status.Type);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _jobItemToUpdate.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationQuoted, _jobItemToUpdate.Location.Type);
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
				_userContext.GetCurrentUser(), _jobItemToUpdateId, 0, 0, "Item quoted on QR2000", ListItemType.StatusQuotedPrepared, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationQuoted));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_quoteItemService = QuoteItemServiceTestHelper.CreateQuoteItemService(
				_userContext,
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith1Item(quoteId),
				quoteItemRepositoryMock,
				jobItemRepositoryMock,
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, jobItemRepositoryMock);
			CreateQuoteItem(id, quoteId, _jobItemToUpdateId, labour, calibration, parts, carriage, investigation, report, days, ber);
			jobItemRepositoryMock.VerifyAllExpectations();
			quoteItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedQuoteItem.Id);
			Assert.AreEqual(2, _savedQuoteItem.ItemNo);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _savedQuoteItem.Status.Type);
			Assert.AreEqual(ListItemType.StatusQuotedPrepared, _jobItemToUpdate.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationQuoted, _jobItemToUpdate.Location.Type);
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
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member),
				QuoteItemServiceTestHelper.GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(quoteId),
				MockRepository.GenerateStub<IQuoteItemRepository>(),
				QuoteItemServiceTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				QuoteItemServiceTestHelper.GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted(),
				MockRepository.GenerateStub<ICustomerRepository>());
			_jobItemService = QuoteItemServiceTestHelper.CreateJobItemService(_userContext, MockRepository.GenerateStub<IJobItemRepository>());
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
	}
}