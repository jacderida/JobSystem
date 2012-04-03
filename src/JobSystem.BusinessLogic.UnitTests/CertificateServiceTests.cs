using System;
using System.Collections.Generic;
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
using JobSystem.TestHelpers.RepositoryHelpers;
using JobSystem.Resources.Certificates;

namespace JobSystem.BusinessLogic.UnitTests
{
	public class CertificateServiceTests
	{
		private IUserContext _userContext;
		private DomainValidationException _domainValidationException;
		private CertificateService _certificateService;
		private Certificate _savedCertificate;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		[SetUp]
		public void Setup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_domainValidationException = null;
			_savedCertificate = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
		}

		[Test]
		public void Create_ValidCertificateDetails_CertificateCreated()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			var certificateRepositoryMock = MockRepository.GenerateMock<ICertificateRepository>();
			certificateRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateType(certificateTypeId),
				certificateRepositoryMock,
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
			certificateRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedCertificate.Id);
			Assert.That(_savedCertificate.CertificateNumber.StartsWith("CERT"));
			Assert.AreEqual(_dateCreated, _savedCertificate.DateCreated);
			Assert.AreEqual("graham.robertson@intertek.com", _savedCertificate.CreatedBy.EmailAddress);
			Assert.IsNotNull(_savedCertificate.JobItem);
			Assert.IsNotNull(_savedCertificate.Type);
			Assert.That(_savedCertificate.TestStandards.Count > 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrow()
		{
			var id = Guid.Empty;
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateType(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidTypeId_ArgumentExceptionThrow()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNull(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NonCertificateListItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonCertificateType(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_JobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateType(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidTestStandardId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.Empty };

			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateType(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
		}

		[Test]
		public void Create_ProcedureListGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = new string('a', 256);
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			_certificateService = CertificateServiceFactory.Create(
				_userContext,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateType(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ProcedureListTooLarge));
		}

		[Test]
		public void Create_UserHasInsufficientClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var certificateTypeId = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var procedureList = "001; 002";
			var testStandards = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };

			_certificateService = CertificateServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateType(certificateTypeId),
				MockRepository.GenerateStub<ICertificateRepository>(),
				JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId),
				TestStandardRepositoryHelper.GetTestStandardRepository_StubsGetById_ReturnsTestStandards(testStandards));
			Create(id, certificateTypeId, jobItemId, procedureList, testStandards);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		public void Create(Guid id, Guid certificateTypeId, Guid jobItemId, string procedureList, List<Guid> testStandardIds)
		{
			try
			{
				_savedCertificate = _certificateService.Create(id, certificateTypeId, jobItemId, procedureList, testStandardIds);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}