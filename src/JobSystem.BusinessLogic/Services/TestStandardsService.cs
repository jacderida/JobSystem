using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.TestStandards;

namespace JobSystem.BusinessLogic.Services
{
	public class TestStandardsService : ServiceBase
	{
		private readonly ITestStandardRepository _testStandardRepository;

		public TestStandardsService(
			IUserContext applicationContext,
			ITestStandardRepository testStandardRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_testStandardRepository = testStandardRepository;
		}

		public TestStandard Create(Guid id, string description, string serialNo, string certificateNo)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the test standard");
			var testStandard = new TestStandard();
			testStandard.Id = id;
			testStandard.Description = description;
			testStandard.SerialNo = serialNo;
			testStandard.CertificateNo = certificateNo;
			ValidateAnnotatedObjectThrowOnFailure(testStandard);
			_testStandardRepository.Create(testStandard);
			return testStandard;
		}

		public TestStandard Edit(Guid id, string description, string serialNo, string certificateNo)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			var testStandard = _testStandardRepository.GetById(id);
			if (testStandard == null)
				throw new ArgumentException("A valid ID must be supplied for the test standard");
			testStandard.Description = description;
			testStandard.SerialNo = serialNo;
			testStandard.CertificateNo = certificateNo;
			ValidateAnnotatedObjectThrowOnFailure(testStandard);
			_testStandardRepository.Update(testStandard);
			return testStandard;
		}

		public TestStandard GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _testStandardRepository.GetById(id);
		}

		public IEnumerable<TestStandard> GetTestStandards()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _testStandardRepository.GetTestStandards();
		}
	}
}