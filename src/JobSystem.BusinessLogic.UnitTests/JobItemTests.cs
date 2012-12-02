using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class JobItemServiceTests
	{
		private JobItemService _jobItemService;
		private DomainValidationException _domainValidationException;
		private JobItem _savedJobItem;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);
		
		private JobItem _jobItemToUpdate;
		private Guid _jobItemToUpdateId;
		
		private JobItem _jobItemToUpdateJobNotApproved;
		private Guid _jobItemToUpdateJobNotApprovedId;

		private JobItem _jobItemForEditInformation;
		private Guid _jobItemForEditInformationId;

		private IUserContext _userContext;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Admin | UserRole.Manager | UserRole.Member);
		}

		[SetUp]
		public void Setup()
		{
			_jobItemToUpdateId = Guid.NewGuid();
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
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
			_jobItemToUpdateJobNotApproved = new JobItem
			{
				Id = _jobItemToUpdateJobNotApprovedId,
				Job = new Job
				{
					Id = Guid.NewGuid(),
					JobNo = "JR2000",
					CreatedBy = _userContext.GetCurrentUser(),
					OrderNo = "ORDER12345",
					DateCreated = DateTime.UtcNow,
					Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
					IsPending = true
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

			_jobItemForEditInformationId = Guid.NewGuid();
			_jobItemForEditInformation = new JobItem
			{
				Id = _jobItemForEditInformationId,
				Job = new Job
				{
					Id = Guid.NewGuid(),
					JobNo = "JR2000",
					CreatedBy = _userContext.GetCurrentUser(),
					OrderNo = "ORDER12345",
					DateCreated = DateTime.UtcNow,
					Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
					IsPending = true
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
			var fieldId = Guid.NewGuid();

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(
				jobItemRepositoryMock, jobId, instrumentId, initialStatusId, fieldId, 0, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));

			//_jobItemService = JobItemServiceFactory.Create(
			//    jobItemRepositoryMock, jobId, instrumentId, initialStatusId, fieldId, 0, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
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
			var fieldId = Guid.NewGuid();

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(
				jobItemRepositoryMock, jobId, instrumentId, initialStatusId, fieldId, 1, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
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
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the job item ID")]
		public void CreateJobItem_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.Empty;
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the instrument ID")]
		public void CreateJobItem_InvalidInstrumentId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.Empty;
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the list item ID")]
		public void CreateJobItem_InvalidInitialStatusId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.Empty;
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid ID must be supplied for the list item ID")]
		public void CreateJobItem_InvalidFieldId_ArgumentExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.Empty;

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
		}

		[Test]
		public void CreateJobItem_InvalidCalPeriod_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER12345", "AS123", initialStatusId, fieldId, 0, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidCalPeriod));
		}

		[Test]
		public void CreateJobItem_SerialNoNotSupplied_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, String.Empty, "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.SerialNoRequired));
		}

		[Test]
		public void CreateJobItem_SerialNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, new string('a', 51), "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.SerialNoTooLarge));
		}

		[Test]
		public void CreateJobItem_AssetNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", new string('a', 51), initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.AssetNoTooLarge));
		}

		[Test]
		public void CreateJobItem_InstructionsGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, fieldId, 12, new string('a', 256), "job item accessories", false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InstructionsTooLarge));
		}

		[Test]
		public void CreateJobItem_AccessoriesGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, fieldId, 12, "job item instructions", new string('a', 256), false, "job item returned", "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.AccessoriesTooLarge));
		}

		[Test]
		public void CreateJobItem_ReturnReasonGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, new string('a', 256), "job item comments");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ReturnReasonTooLarge));
		}

		[Test]
		public void CreateJobItem_CommentsGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var jobId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var instrumentId = Guid.NewGuid();
			var initialStatusId = Guid.NewGuid();
			var fieldId = Guid.NewGuid();

			_jobItemService = JobItemServiceFactory.Create(jobId, instrumentId, initialStatusId, fieldId, 0);
			CreateJobItem(jobId, jobItemId, instrumentId, "SER123", "AS123", initialStatusId, fieldId, 12, "job item instructions", "job item accessories", false, "job item accessories", new string('a', 256));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.CommentsTooLarge));
		}

		private void CreateJobItem(
			Guid jobId, Guid jobItemId, Guid instrumentId, string serialNo, string assetNo, Guid initialStatusId, Guid fieldId, int calPeriod,
			string instructions, string accessories, bool isReturned, string returnReason, string comments)
		{
			try
			{
				_savedJobItem = _jobItemService.CreateJobItem(
					jobId, jobItemId, instrumentId, serialNo, assetNo, initialStatusId, fieldId, calPeriod, instructions, accessories, isReturned, returnReason, comments);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region AddWorkItem

		[Test]
		public void AddWorkItem_ValidWorkItemDetails_WorkItemAdded()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdateId, workTime, overTime, report, ListItemType.WorkStatusCalibrated, ListItemType.WorkTypeCalibration));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate));
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryMock, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(ListItemType.WorkStatusCalibrated, _savedJobItem.Status.Type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidJobItemIdSupplied_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(Guid.NewGuid())).Return(null);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
		}

		[Test]
		public void AddWorkItem_InvalidWorkTimeSupplied_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = -2;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ItemHistoryInvalidWorkTime));
		}

		[Test]
		public void AddWorkItem_InvalidOverTimeSupplied_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = -10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ItemHistoryInvalidOverTime));
		}

		[Test]
		public void AddWorkItem_InvalidReportSupplied_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = new string('a', 256);

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.ItemHistoryReportTooLarge));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidStatusId_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.Empty;
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddWorkItem_InvalidWorkTypeId_ArgumentExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.Empty;
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
		}

		[Test]
		public void AddWorkItem_StatusWithInvalidCategory_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, GetListItemRepositoryForInvalidStatusCategory(workStatusId, workTypeId), _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidStatusCategory));
		}

		[Test]
		public void AddWorkItem_WorkTypeWithInvalidCategory_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, GetListItemRepositoryForInvalidWorkTypeCategory(workStatusId, workTypeId), _userContext);
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InvalidWorkTypeCategory));
		}

		[Test]
		public void AddWorkItem_CurrentUserHasInsufficientSecurity_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateId)).Return(_jobItemToUpdate);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(
				jobItemRepositoryStub, workStatusId, workTypeId, TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			AddWorkItem(_jobItemToUpdateId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InsufficientSecurityClearance));
		}

		[Test]
		public void AddWorkItem_JobNotApproved_DomainValidationExceptionThrown()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workTime = 25;
			var overTime = 10;
			var report = "Instrument calibrated OK";

			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdateJobNotApprovedId)).Return(_jobItemToUpdateJobNotApproved);
			_jobItemService = JobItemServiceFactory.CreateForAddWorkItem(jobItemRepositoryStub, workStatusId, workTypeId, _userContext);
			AddWorkItem(_jobItemToUpdateJobNotApprovedId, workTime, overTime, report, workStatusId, workTypeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.JobNotApproved));
		}

		private void AddWorkItem(Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId)
		{
			try
			{
				_savedJobItem = _jobItemService.AddWorkItem(jobItemId, workTime, overTime, report, workStatusId, workTypeId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		private static IListItemRepository GetListItemRepositoryForInvalidStatusCategory(Guid workStatusId, Guid workTypeId)
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
			return listItemRepositoryStub;
		}

		private static IListItemRepository GetListItemRepositoryForInvalidWorkTypeCategory(Guid workStatusId, Guid workTypeId)
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
			return listItemRepositoryStub;
		}

		#endregion
		#region EditInformation

		[Test]
		public void EditInformation_ValidInformation_JobItemEdited()
		{
			var instructions = "edited instructions";
			var accessories = "edited accessories";
			var comments = "edited comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			jobItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);

			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_jobItemForEditInformationId, _jobItemForEditInformation.Id);
			Assert.AreEqual(instructions, _jobItemForEditInformation.Instructions);
			Assert.AreEqual(accessories, _jobItemForEditInformation.Accessories);
			Assert.AreEqual(comments, _jobItemForEditInformation.Comments);
		}

		[Test]
		public void EditInformation_UserIsManager_JobItemEdited()
		{
			var instructions = "edited instructions";
			var accessories = "edited accessories";
			var comments = "edited comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			jobItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member),
				jobItemRepositoryMock);

			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_jobItemForEditInformationId, _jobItemForEditInformation.Id);
			Assert.AreEqual(instructions, _jobItemForEditInformation.Instructions);
			Assert.AreEqual(accessories, _jobItemForEditInformation.Accessories);
			Assert.AreEqual(comments, _jobItemForEditInformation.Comments);
		}

		[Test]
		public void EditInformation_UserIsAdmin_JobItemEdited()
		{
			var instructions = "edited instructions";
			var accessories = "edited accessories";
			var comments = "edited comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			jobItemRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_jobItemService = JobItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Admin | UserRole.Member),
				jobItemRepositoryMock);

			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_jobItemForEditInformationId, _jobItemForEditInformation.Id);
			Assert.AreEqual(instructions, _jobItemForEditInformation.Instructions);
			Assert.AreEqual(accessories, _jobItemForEditInformation.Accessories);
			Assert.AreEqual(comments, _jobItemForEditInformation.Comments);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EditInformation_InvalidJobId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instructions = "edited instructions";
			var accessories = "edited accessories";
			var comments = "edited comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(id)).Return(null);
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			EditInformation(id, instructions, accessories, comments);
		}

		[Test]
		public void EditInformation_InvalidInstructions_DomainValidationExceptionThrown()
		{
			var instructions = new String('a', 257);
			var accessories = "edited accessories";
			var comments = "edited comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InstructionsTooLarge));
		}

		[Test]
		public void EditInformation_InvalidAccessories_DomainValidationExceptionThrown()
		{
			var instructions = "some instructions";
			var accessories = new String('a', 256);
			var comments = "edited comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.AccessoriesTooLarge));
		}

		[Test]
		public void EditInformation_InvalidComments_DomainValidationExceptionThrown()
		{
			var instructions = "some instructions";
			var accessories = "some accessories";
			var comments = new String('a', 256);

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			_jobItemService = JobItemServiceFactory.Create(_userContext, jobItemRepositoryMock);
			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.CommentsTooLarge));
		}

		[Test]
		public void EditInformation_InsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var instructions = "some instructions";
			var accessories = "some accessories";
			var comments = "some comments";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemForEditInformationId)).Return(_jobItemForEditInformation);
			_jobItemService = JobItemServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member),
				jobItemRepositoryMock);
			EditInformation(_jobItemForEditInformationId, instructions, accessories, comments);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.JobItems.Messages.InsufficientSecurityClearance));
		}

		private void EditInformation(Guid jobItemId, string instructions, string accessories, string comments)
		{
			try
			{
				_jobItemForEditInformation = _jobItemService.EditInformation(jobItemId, instructions, accessories, comments);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}