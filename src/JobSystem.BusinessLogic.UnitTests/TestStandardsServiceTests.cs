using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.TestStandards;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;

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
		private TestStandard _testStandardForEdit;

		[SetUp]
		public void Setup()
		{
			_savedTestStandard = null;
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_testStandardForEditId = Guid.NewGuid();
			_testStandardForEdit = new TestStandard()
			{
				Id = _testStandardForEditId,
				Description = "some test standard",
				SerialNo = "some serial no",
				CertificateNo = "some cert no"
			};
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

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var description = "some description";
			var serialNo = "serialno123456";
			var certificateNo = "certno12345";

			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			CreateTestStandard(id, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
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
		#region Edit

		[Test]
		public void Edit_ValidTestStandardDetails_ItemSuccessfullyEdited()
		{
			var description = "edited description";
			var serialNo = "edited serialno123456";
			var certificateNo = "edited some certificate no";

			var testStandardRepositoryMock = MockRepository.GenerateMock<ITestStandardRepository>();
			testStandardRepositoryMock.Stub(x => x.GetById(_testStandardForEditId)).Return(_testStandardForEdit);
			testStandardRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_testStandardsService = TestStandardServiceFactory.Create(_userContext, testStandardRepositoryMock);
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			testStandardRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(description, _testStandardForEdit.Description);
			Assert.AreEqual(serialNo, _testStandardForEdit.SerialNo);
			Assert.AreEqual(certificateNo, _testStandardForEdit.CertificateNo);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidTestStandardId_ArgumentExceptionThrown()
		{
			var description = "edited description";
			var serialNo = "edited serialno123456";
			var certificateNo = "edited some certificate no";

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsNull(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
		}

		[Test]
		public void Edit_DescriptionEmpty_DomainValidationExceptionThrown()
		{
			var description = String.Empty;
			var serialNo = "edited serialno123456";
			var certificateNo = "edited some certificate no";

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionNotSupplied));
		}

		[Test]
		public void Edit_DescriptionTooLarge_DomainValidationExceptionThrown()
		{
			var description = new String('a', 256);
			var serialNo = "edited serialno123456";
			var certificateNo = "edited some certificate no";

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLarge));
		}

		[Test]
		public void Edit_SerialNoEmpty_DomainValidationExceptionThrown()
		{
			var description = "edited description";
			var serialNo = String.Empty;
			var certificateNo = "edited some certificate no";

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.SerialNoNotSupplied));
		}

		[Test]
		public void Edit_SerialNoTooLarge_DomainValidationExceptionThrown()
		{
			var description = "edited description";
			var serialNo = new String('a', 51);
			var certificateNo = "edited some certificate no";

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.SerialNoTooLarge));
		}

		[Test]
		public void Edit_CertNoEmpty_DomainValidationExceptionThrown()
		{
			var description = "edited description";
			var serialNo = "edited serial no";
			var certificateNo = String.Empty;

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.CertNoNotSupplied));
		}

		[Test]
		public void Edit_CertNoTooLarge_DomainValidationExceptionThrown()
		{
			var description = "edited description";
			var serialNo = "edited serial no";
			var certificateNo = new String('a', 51);

			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.CertNoTooLarge));
		}

		[Test]
		public void Edit_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var description = "edited description";
			var serialNo = "edited serial no";
			var certificateNo = "edited cert no";

			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
			_testStandardsService = TestStandardServiceFactory.Create(
				_userContext, TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandard(_testStandardForEditId));
			EditTestStandard(_testStandardForEditId, description, serialNo, certificateNo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void EditTestStandard(Guid id, string description, string serialNo, string certificateNo)
		{
			try
			{
				_testStandardForEdit = _testStandardsService.Edit(id, description, serialNo, certificateNo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetById

		[Test]
		public void GetById_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public);
			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			GetById(id);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void GetById(Guid id)
		{
			try
			{
				_testStandardsService.GetById(id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetTestStandards

		[Test]
		public void GetTestStandards_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public);
			_testStandardsService = TestStandardServiceFactory.Create(_userContext, MockRepository.GenerateStub<ITestStandardRepository>());
			GetTestStandards();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void GetTestStandards()
		{
			try
			{
				_testStandardsService.GetTestStandards();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}