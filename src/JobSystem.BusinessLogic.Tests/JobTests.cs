using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.Framework;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class JobTests
	{
		private JobService _jobService;
		private DomainValidationException _domainValidationException;
		private Job _savedJob;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () =>_dateCreated;
		}

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
	}
}