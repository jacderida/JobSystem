using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.BusinessLogic.Tests.Helpers;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class JobTests
	{
		private JobService _jobService;
		private DomainValidationException _domainValidationException;
		private Job _savedJob;

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
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