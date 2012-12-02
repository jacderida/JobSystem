using System;
using System.IO;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Storage.Jobs;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class JobServiceTests
	{
		private JobService _jobService;
		private DomainValidationException _domainValidationException;
		private Job _savedJob;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);
		private Guid _jobForAttachmentId = Guid.NewGuid();
		private Job _jobForAttachment;
		private IUserContext _userContext;
		private Guid _jobForEditId;
		private Job _jobForEdit;

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Admin | UserRole.Manager);
			_jobForAttachment = new Job
			{
				Id = _jobForAttachmentId,
				Type = new ListItem
				{
					Id = Guid.NewGuid(),
					Type = ListItemType.JobTypeField,
					Name = "Field",
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Type", Type = ListItemCategoryType.JobType }
				},
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				OrderNo = "blah",
				DateCreated = DateTime.Now,
				CreatedBy = _userContext.GetCurrentUser(),
				JobNo = "JR2000"
			};
			_jobForEditId = Guid.NewGuid();
			_jobForEdit = new Job
			{
				Id = _jobForEditId,
				Type = new ListItem
				{
					Id = Guid.NewGuid(),
					Type = ListItemType.JobTypeField,
					Name = "Field",
					Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Type", Type = ListItemCategoryType.JobType }
				},
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				OrderNo = "orderno",
				AdviceNo = "adviceno",
				Notes = "notes",
				Contact = "contact",
				Instructions = "instructions",
				DateCreated = DateTime.Now,
				CreatedBy = _userContext.GetCurrentUser(),
				JobNo = "JR2001"
			};
		}

		#region Create

		[Test]
		public void Create_ValidJobDetails_SuccessfullyCreateJob()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_jobService = JobServiceFactory.Create(jobRepositoryMock, typeId, customerId);
			CreateJob(id, "job instructions", "PO1000", "AD1000", typeId, customerId, "some notes", "job contact");
			jobRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedJob.Id == id);
			Assert.That(!String.IsNullOrEmpty(_savedJob.JobNo) && _savedJob.JobNo.EndsWith("JR"));
			Assert.IsTrue(_savedJob.IsPending);
			Assert.AreEqual(_savedJob.DateCreated, _dateCreated);
			Assert.AreEqual("test@usercontext.com", _savedJob.CreatedBy.EmailAddress);
		}

		[Test]
		public void Create_InstructionsGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, new string('A', 2001), "PO1000", "AD1000", typeId, customerId, "some notes", "job contact");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InstructionsTooLarge));
		}

		[Test]
		public void Create_OrderNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, "job instructions", new string('a', 51), "AD1000", typeId, customerId, "some notes", "job contact");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.OrderNoTooLarge));
		}

		[Test]
		public void Create_AdviceNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, "job instructions", "PO1000", new string('a', 51), typeId, customerId, "some notes", "job contact");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.AdviceNoTooLarge));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "An invalid type ID was supplied")]
		public void Create_InvalidTypeIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.Empty;

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, "job instructions", "PO1000", "AD1000", typeId, customerId, "some notes", "job contact");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "An invalid customer ID was supplied")]
		public void Create_InvalidCustomerIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.Empty;
			var typeId = Guid.NewGuid();

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, "job instructions", "PO1000", "AD1000", typeId, customerId, "some notes", "job contact");
		}

		[Test]
		public void Create_NotesGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, "job instructions", "PO1000", "AD1000", typeId, customerId, new string('a', 2001), "job contact");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.NotesTooLarge));
		}

		[Test]
		public void Create_ContactGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			_jobService = JobServiceFactory.Create(typeId, customerId);
			CreateJob(id, "job instructions", "PO1000", "AD1000", typeId, customerId, "some notes", new string('a', 51));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.ContactTooLarge));
		}

		private void CreateJob(
			Guid id, string instructions, string orderNo, string adviceNo, Guid typeId, Guid customerId, string notes, string contact)
		{
			try
			{
				_savedJob = _jobService.CreateJob(id, instructions, orderNo, adviceNo, typeId, customerId, notes, contact);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Approve

		[Test]
		public void Approve_UserHasSufficientSecurityClearance_JobSuccessfullyApproved()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			var jobRepository = MockRepository.GenerateMock<IJobRepository>();
			jobRepository.Stub(x => x.GetJobItemCount(id)).Return(1);
			jobRepository.Expect(x => x.Update(null)).IgnoreArguments();
			_jobService = JobServiceFactory.CreateForApproval(jobRepository, id, typeId, customerId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.JobApprover));
			ApproveJob(id);
			jobRepository.VerifyAllExpectations();
			Assert.That(!_savedJob.IsPending);
		}

		[Test]
		public void Approve_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			var jobRepository = MockRepository.GenerateMock<IJobRepository>();
			_jobService = JobServiceFactory.CreateForApproval(jobRepository, id, typeId, customerId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
			ApproveJob(id);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Approve_JobIdNotSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			var jobRepository = MockRepository.GenerateMock<IJobRepository>();
			_jobService = JobServiceFactory.CreateForApproval(jobRepository, id, typeId, customerId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.JobApprover));
			ApproveJob(id);
		}

		[Test]
		public void Approve_JobHasNoItems_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var typeId = Guid.NewGuid();

			var jobRepository = MockRepository.GenerateStub<IJobRepository>();
			jobRepository.Stub(x => x.GetJobItemCount(id)).Return(0);
			_jobService = JobServiceFactory.CreateForApproval(jobRepository, id, typeId, customerId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.JobApprover));
			ApproveJob(id);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.ApprovingJobHasNoItems));
		}

		private void ApproveJob(Guid id)
		{
			try
			{
				_savedJob = _jobService.ApproveJob(id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region AddAttachment

		[Test]
		public void AddAttachment_ValidAttachmentDetails_AttachmentAdded()
		{
			var attachmentId = Guid.NewGuid();
			var fileName = "attachment.pdf";
			var contentType = "image";
			var content = new byte[] { 1, 2, 3, 4, 5 };

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			jobRepositoryMock.Stub(x => x.GetById(_jobForAttachmentId)).Return(_jobForAttachment);
			_jobService = JobServiceFactory.Create(jobRepositoryMock, MockRepository.GenerateStub<IJobAttachmentDataRepository>());
			AddAttachment(_jobForAttachmentId, attachmentId, fileName, contentType, content);
			jobRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(1, _jobForAttachment.Attachments.Count);
		}

		[Test]
		public void AddAttachment_FilenameNotSupplied_DomainValidationExceptionThrow()
		{
			var attachmentId = Guid.NewGuid();
			var fileName = String.Empty;
			var contentType = "image";
			var content = new byte[] { 1, 2, 3, 4, 5 };

			var jobRepositoryStub = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryStub.Stub(x => x.GetById(_jobForAttachmentId)).Return(_jobForAttachment);
			_jobService = JobServiceFactory.Create(jobRepositoryStub, MockRepository.GenerateStub<IJobAttachmentDataRepository>());
			AddAttachment(_jobForAttachmentId, attachmentId, fileName, contentType, content);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.FileNameRequired));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddAttachment_AttachmentIdNotSupplied_ArgumentExceptionThrow()
		{
			var attachmentId = Guid.Empty;
			var fileName = "attachment.pdf";
			var contentType = "image";
			byte[] content = new byte[] { 1, 2, 3, 4, 5 };

			var jobRepositoryStub = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryStub.Stub(x => x.GetById(_jobForAttachmentId)).Return(_jobForAttachment);
			_jobService = JobServiceFactory.Create(jobRepositoryStub, MockRepository.GenerateStub<IJobAttachmentDataRepository>());
			AddAttachment(_jobForAttachmentId, attachmentId, fileName, contentType, content);
		}

		[Test]
		public void AddAttachment_UserDoesNotHaveMemberRole_DomainValidationExceptionThrow()
		{
			var attachmentId = Guid.NewGuid();
			var fileName = "attachment.pdf";
			var contentType = "image";
			byte[] content = new byte[] { 1, 2, 3, 4, 5 };

			var jobRepositoryStub = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryStub.Stub(x => x.GetById(_jobForAttachmentId)).Return(_jobForAttachment);
			_jobService = JobServiceFactory.Create(jobRepositoryStub, MockRepository.GenerateStub<IJobAttachmentDataRepository>(),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			AddAttachment(_jobForAttachmentId, attachmentId, fileName, contentType, content);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddAttachment_InvalidJobIdSupplied_ArgumentExceptionThrow()
		{
			var attachmentId = Guid.NewGuid();
			var fileName = "attachment.pdf";
			var contentType = "image";
			byte[] content = new byte[] { 1, 2, 3, 4, 5 };

			var jobRepositoryStub = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryStub.Stub(x => x.GetById(Guid.Empty)).Return(null);
			_jobService = JobServiceFactory.Create(jobRepositoryStub, MockRepository.GenerateStub<IJobAttachmentDataRepository>());
			AddAttachment(_jobForAttachmentId, attachmentId, fileName, contentType, content);
		}

		private void AddAttachment(Guid jobId, Guid attachmentId, string fileName, string contentType, byte[] content)
		{
			try
			{
				MemoryStream ms = null;
				if (content != null)
					ms = new MemoryStream(content);
				_jobForAttachment = _jobService.AddAttachment(jobId, attachmentId, fileName);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Get

		[Test]
		public void GetJob_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			try
			{
				var customerId = Guid.NewGuid();
				var typeId = Guid.NewGuid();
				var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
				_jobService = JobServiceFactory.Create(
					jobRepositoryMock, typeId, customerId, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
				_jobService.GetJob(Guid.NewGuid());
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
		}

		[Test]
		public void GetApprovedJobs_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			try
			{
				var customerId = Guid.NewGuid();
				var typeId = Guid.NewGuid();
				var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
				_jobService = JobServiceFactory.Create(
					jobRepositoryMock, typeId, customerId, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
				_jobService.GetApprovedJobs();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
		}

		[Test]
		public void GetPendingJobs_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			try
			{
				var customerId = Guid.NewGuid();
				var typeId = Guid.NewGuid();
				var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
				_jobService = JobServiceFactory.Create(
					jobRepositoryMock, typeId, customerId, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
				_jobService.GetPendingJobs();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
		}

		#endregion
		#region Edit

		[Test]
		public void Edit_ValidJobDetails_SuccessfullyEditJob()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			jobRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			
			jobRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_jobForEdit.Id, _jobForEditId);
			Assert.AreEqual(_jobForEdit.OrderNo, orderNumber);
			Assert.AreEqual(_jobForEdit.AdviceNo, adviceNumber);
			Assert.AreEqual(_jobForEdit.Notes, notes);
			Assert.AreEqual(_jobForEdit.Instructions, instructions);
		}

		[Test]
		public void Edit_ManagerUserContext_SuccessfullyEditJob()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			jobRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_jobService = JobServiceFactory.Create(
				jobRepositoryMock,
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member));
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);

			jobRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_jobForEdit.Id, _jobForEditId);
			Assert.AreEqual(_jobForEdit.OrderNo, orderNumber);
			Assert.AreEqual(_jobForEdit.AdviceNo, adviceNumber);
			Assert.AreEqual(_jobForEdit.Notes, notes);
			Assert.AreEqual(_jobForEdit.Instructions, instructions);
		}

		[Test]
		public void Edit_AdminUserContext_SuccessfullyEditJob()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			jobRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_jobService = JobServiceFactory.Create(
				jobRepositoryMock,
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Admin | UserRole.Member));
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);

			jobRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_jobForEdit.Id, _jobForEditId);
			Assert.AreEqual(_jobForEdit.OrderNo, orderNumber);
			Assert.AreEqual(_jobForEdit.AdviceNo, adviceNumber);
			Assert.AreEqual(_jobForEdit.Notes, notes);
			Assert.AreEqual(_jobForEdit.Instructions, instructions);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidJobId_ThrowsArgumentException()
		{
			var id = Guid.NewGuid();
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(id)).Return(null);
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
		}

		[Test]
		public void Edit_OrderNoGreaterThan50Characters_ThrowsDomainValidationException()
		{
			var orderNumber = new String('a', 51);
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.OrderNoTooLarge));
		}

		[Test]
		public void Edit_AdviceNoGreaterThan50Characters_ThrowsDomainValidationException()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = new String('a', 51);
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.AdviceNoTooLarge));
		}

		[Test]
		public void Edit_ContactGreaterThan50Characters_ThrowsDomainValidationException()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = new String('a', 51);
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.ContactTooLarge));
		}

		[Test]
		public void Edit_NotesGreaterThan2000Characters_ThrowsDomainValidationException()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = new String('a', 2001);
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.NotesTooLarge));
		}

		[Test]
		public void Edit_InstructionsGreaterThan2000Characters_ThrowsDomainValidationException()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = new String('a', 2001);

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			_jobService = JobServiceFactory.Create(jobRepositoryMock);
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InstructionsTooLarge));
		}

		[Test]
		public void Edit_UserHasInsufficientSecurityClearance_ThrowsDomainValidationException()
		{
			var orderNumber = "orderno-edited";
			var adviceNumber = "adviceno-edited";
			var contact = "contact-edited";
			var notes = "notes-edited";
			var instructions = "instructions-edited";

			var jobRepositoryMock = MockRepository.GenerateMock<IJobRepository>();
			jobRepositoryMock.Stub(x => x.GetById(_jobForEditId)).Return(_jobForEdit);
			_jobService = JobServiceFactory.Create(
				jobRepositoryMock,
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			Edit(_jobForEditId, orderNumber, adviceNumber, contact, notes, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
		}

		private void Edit(Guid id, string orderNumber, string adviceNumber, string contact, string notes, string instructions)
		{
			try
			{
				_jobForEdit = _jobService.Edit(id, orderNumber, adviceNumber, contact, notes, instructions);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}