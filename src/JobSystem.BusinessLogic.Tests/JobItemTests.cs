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



		#endregion
	}
}