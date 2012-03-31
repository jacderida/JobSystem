using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

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
	}
}