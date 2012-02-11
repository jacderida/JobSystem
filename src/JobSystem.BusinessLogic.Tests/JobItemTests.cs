using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class JobItemTests
	{
		private JobItemService _jobItemService;
		private DomainValidationException _domainValidationException;
		private JobItem _savedJobItem;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);
		private JobItem _jobItemToUpdate;
		private Guid _jobItemToUpdateId;
		private IUserContext _userContext;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
		}

		[SetUp]
		public void Setup()
		{
			_jobItemToUpdateId = Guid.NewGuid();
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_jobItemToUpdate = new JobItem
			{
				Job = new Job
				{
					Id = _jobItemToUpdateId,
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
			_savedJobItem = null;
		}

		#region Create

		[Test]
		public void CreateJobItem_ValidJobItemDetailsWithFirstItem_JobItemCreatedSuccessfullyWithItemNoAs1()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(
				jobItemRepositoryMock, jobId, instrumentId, initialStatusId, locationId, fieldId, 0, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedJobItem.Id != Guid.Empty);
			Assert.AreEqual(1, _savedJobItem.ItemNo);
			Assert.AreEqual(_dateCreated, _savedJobItem.Created);
			Assert.AreEqual("graham.robertson@intertek.com", _savedJobItem.CreatedUser.EmailAddress);
			Assert.AreEqual("Booked In", _savedJobItem.Status.Name);
		}

		[Test]
		public void CreateJobItem_ValidJobItemDetailsWithFirstItem_JobItemCreatedSuccessfullyWithItemNoAs2()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(
				jobItemRepositoryMock, jobId, instrumentId, initialStatusId, locationId, fieldId, 1, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedJobItem.Id != Guid.Empty);
			Assert.AreEqual(2, _savedJobItem.ItemNo);
			Assert.AreEqual(_dateCreated, _savedJobItem.Created);
			Assert.AreEqual("graham.robertson@intertek.com", _savedJobItem.CreatedUser.EmailAddress);
			Assert.AreEqual("Booked In", _savedJobItem.Status.Name);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the job ID")]
		public void CreateJobItem_InvalidJobId_ArgumentExceptionThrown()
		{
			var jobId = Guid.Empty;
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the job item ID")]
		public void CreateJobItem_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.Empty;
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the instrument ID")]
		public void CreateJobItem_InvalidInstrumentId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.Empty;
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the list item ID")]
		public void CreateJobItem_InvalidInitialStatusId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.Empty;
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the list item ID")]
		public void CreateJobItem_InvalidLocationId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.Empty;
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the list item ID")]
		public void CreateJobItem_InvalidFieldId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.Empty;

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		public void CreateJobItem_InvalidCalPeriod_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, locationId, fieldId, 0, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidCalPeriod));
		}

		[Test]
		public void CreateJobItem_SerialNoNotSupplied_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, String.Empty, "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.SerialNoRequired));
		}

		[Test]
		public void CreateJobItem_SerialNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, new string('a', 51), "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.SerialNoTooLarge));
		}

		[Test]
		public void CreateJobItem_AssetNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", new string('a', 51), initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.AssetNoTooLarge));
		}

		[Test]
		public void CreateJobItem_InstructionsGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, locationId, fieldId, 12, new string('a', 256), "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InstructionsTooLarge));
		}

		[Test]
		public void CreateJobItem_AccessoriesGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", new string('a', 256), false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.AccessoriesTooLarge));
		}

		[Test]
		public void CreateJobItem_ReturnReasonGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, new string('a', 256), "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ReturnReasonTooLarge));
		}

		[Test]
		public void CreateJobItem_CommentsGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var locationId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, locationId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, locationId, fieldId, 12, "job item instructions", "job item accessories", false, "job item accessories", new string('a', 256));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.CommentsTooLarge));
		}

		private void CreateJobItem(
			Guid jobId, Guid jobItemId, Guid instrumentId, string serialNo, string assetNo, Guid initialStatusId, Guid locationId, Guid fieldId, int calPeriod,
			string instructions, string accessories, bool isReturned, string returnReason, string comments)
		{
			try
			{
				_savedJobItem = _jobItemService.CreateJobItem(
					jobId, jobItemId, instrumentId, serialNo, assetNo, initialStatusId, locationId, fieldId, calPeriod, instructions, accessories, isReturned, returnReason, comments);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region AddWork

		[Test]
		public void AddWorkItem_ValidWorkItemDetails_WorkItemAdded()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.CreateItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, workTime, overTime, report, ListItemType.WorkStatusCalibrated, ListItemType.WorkTypeCalibration, ListItemType.WorkLocationCalibrated));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate));
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryMock, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(ListItemType.WorkStatusCalibrated, _savedJobItem.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationCalibrated, _savedJobItem.Location.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidJobItemIdSupplied_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(Guid.NewGuid())).Return(null);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
		}

		[Test]
		public void AddWorkItem_InvalidWorkTimeSupplied_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = -2;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ItemHistoryInvalidWorkTime));
		}

		[Test]
		public void AddWorkItem_InvalidOverTimeSupplied_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = -10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ItemHistoryInvalidOverTime));
		}

		[Test]
		public void AddWorkItem_InvalidReportSupplied_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = new string('a', 256);

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ItemHistoryReportTooLarge));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidStatusId_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.Empty;
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidWorkTypeId_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.Empty;
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidLocationId_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.Empty;
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
		}

		[Test]
		public void AddWorkItem_StatusWithInvalidCategory_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, GetListItemRepositoryForInvalidStatusCategory(workStatusId, workTypeId, workLocationId), _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidStatusCategory));
		}

		[Test]
		public void AddWorkItem_WorkTypeWithInvalidCategory_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, GetListItemRepositoryForInvalidWorkTypeCategory(workStatusId, workTypeId, workLocationId), _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidWorkTypeCategory));
		}

		[Test]
		public void AddWorkItem_WorkLocationWithInvalidCategory_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, GetListItemRepositoryForInvalidWorkLocationCategory(workStatusId, workTypeId, workLocationId), _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidWorkLocationCategory));
		}

		[Test]
		public void AddWorkItem_CurrentUserHasInsufficientSecurity_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(
				jobItemRepositoryStub, workStatusId, workLocationId, workTypeId, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InsufficientSecurityClearance));
		}

		private void AddWorkItem(Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			try
			{
				_savedJobItem = _jobItemService.AddWorkItem(jobItemId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		private static IListItemRepository GetListItemRepositoryForInvalidStatusCategory(Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Type = ListItemType.InitialStatusHouseCalibration,
					Name = "House Calibration",
					Category = new ListItemCategory
					{
						Id = Guid.NewGuid(),
						Name = "Initial Status",
						Type = ListItemCategoryType.JobItemInitialStatus
					}
				});
			listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(JobItemServiceFactory.GetAddWorkItemWorkType(workTypeId));
			listItemRepositoryStub.Stub(x => x.GetById(workLocationId)).Return(JobItemServiceFactory.GetAddWorkItemWorkLocation(workLocationId));
			return listItemRepositoryStub;
		}

		private static IListItemRepository GetListItemRepositoryForInvalidWorkTypeCategory(Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(JobItemServiceFactory.GetAddWorkItemWorkStatus(workStatusId));
			listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Type = ListItemType.WorkStatusRepaired,
					Name = "Repaired",
					Category = new ListItemCategory
					{
						Id = Guid.NewGuid(),
						Name = "Status",
						Type = ListItemCategoryType.JobItemWorkStatus
					}
				});
			listItemRepositoryStub.Stub(x => x.GetById(workLocationId)).Return(JobItemServiceFactory.GetAddWorkItemWorkLocation(workLocationId));
			return listItemRepositoryStub;
		}

		private static IListItemRepository GetListItemRepositoryForInvalidWorkLocationCategory(Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(JobItemServiceFactory.GetAddWorkItemWorkStatus(workStatusId));
			listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(JobItemServiceFactory.GetAddWorkItemWorkType(workTypeId));
			listItemRepositoryStub.Stub(x => x.GetById(workLocationId)).Return(
				new ListItem
				{
					Id = Guid.NewGuid(),
					Type = ListItemType.InitialWorkLocationRepair,
					Name = "Repair",
					Category = new ListItemCategory
					{
						Id = Guid.NewGuid(),
						Name = "Initial Location",
						Type = ListItemCategoryType.JobItemInitialLocation
					}
				});
			return listItemRepositoryStub;
		}

		#endregion
	}
}