using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.TestHelpers;
using JobSystem.Resources.TestStandards;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class TestStandardsServiceTests
	{
		private TestStandard _savedTestStandard;
		private TestStandardsService _testStandardsService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);
		private Guid _testStandardForEditId;

		[SetUp]
		public void Setup()
		{
			_savedTestStandard = null;
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
		}

		#region Create

		[Test]
		public void Create_ValidTestStandardDetails_TestStandardCreated()
		{
			var id = Guid.NewGuid();
			var description = "test standard description";
			var serialNo = "123456";
			var certificateNo = "cert123456";

			var testStandardRepositoryMock = MockRepository.GenerateMock<ITestStandardRepository>();
			testStandardRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_testStandardsService = TestStandardServiceFactory.Create(_userContext, testStandardRepositoryMock);
			CreateTestStandard(id, description, serialNo, certificateNo);
			testStandardRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(id, _savedTestStandard.Id);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var description = "test standard description";
			var serialNo = "123456";
			var certificateNo = "cert123456";

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
		}

		[Test]
		public void Create_DescriptionNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = String.Empty;
			var serialNo = "123456";
			var certificateNo = "cert123456";

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionNotSupplied));
		}

		[Test]
		public void Create_DescriptionGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = new string('a', 256);
			var serialNo = "123456";
			var certificateNo = "cert123456";

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLarge));
		}

		[Test]
		public void Create_SerialNoNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = "some description";
			var serialNo = String.Empty;
			var certificateNo = "cert123456";

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.SerialNoNotSupplied));
		}

		[Test]
		public void Create_SerialNoTooLarge_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = "some description";
			var serialNo = new string('a', 51);
			var certificateNo = "cert123456";

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.SerialNoTooLarge));
		}

		[Test]
		public void Create_CertNoNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = "some description";
			var serialNo = "serialno123456";
			var certificateNo = String.Empty;

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.CertNoNotSupplied));
		}

		[Test]
		public void Create_CertNoTooLarge_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = "some description";
			var serialNo = "serialno123456";
			var certificateNo = new String('a', 51);

			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.CertNoTooLarge));
		}

		private void CreateTestStandard(Guid id, string description, string serialNo, string certificateNo)
		{
			try
			{
				_savedTestStandard = _testStandardsService.Create(id, description, serialNo, certificateNo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}