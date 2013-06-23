using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.Certificates;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;

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
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            var certificateRepositoryMock = MockRepository.GenerateMock<ICertificateRepository>();
            certificateRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateAndCategory(certificateTypeId, categoryId),
                certificateRepositoryMock,
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
            certificateRepositoryMock.VerifyAllExpectations();
            Assert.AreNotEqual(Guid.Empty, _savedCertificate.Id);
            Assert.AreEqual("D2000", _savedCertificate.CertificateNumber);
            Assert.AreEqual(_dateCreated, _savedCertificate.DateCreated);
            Assert.AreEqual("graham.robertson@intertek.com", _savedCertificate.CreatedBy.EmailAddress);
            Assert.IsNotNull(_savedCertificate.JobItem);
            Assert.IsNotNull(_savedCertificate.Type);
            Assert.IsNotNull(_savedCertificate.Category);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_IdNotSupplied_ArgumentExceptionThrow()
        {
            var id = Guid.Empty;
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateAndCategory(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_InvalidTypeId_ArgumentExceptionThrow()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNullForCertificate(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_NonCertificateListItemId_ArgumentExceptionThrown()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonCertificateType(certificateTypeId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "A valid category ID must be supplied")]
        public void Create_InvalidCategoryId_ArgumentExceptionThrow()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNullForCategory(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "A category list item must be supplied")]
        public void Create_NonCategoryListItemId_ArgumentExceptionThrown()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonCategoryType(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_JobItemId_ArgumentExceptionThrown()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateAndCategory(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsNull(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
        }

        [Test]
        public void Create_ProcedureListGreaterThan255Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = new string('a', 256);

            _certificateService = CertificateServiceFactory.Create(
                _userContext,
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateAndCategory(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ProcedureListTooLarge));
        }

        [Test]
        public void Create_UserHasInsufficientClearance_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var certificateTypeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var jobItemId = Guid.NewGuid();
            var procedureList = "001; 002";

            _certificateService = CertificateServiceFactory.Create(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsCertificateAndCategory(certificateTypeId, categoryId),
                MockRepository.GenerateStub<ICertificateRepository>(),
                JobItemRepositoryTestHelper.GetJobItemRepository_StubsGetById_ReturnsJobItem(jobItemId));
            Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
        }

        public void Create(Guid id, Guid certificateTypeId, Guid categoryId, Guid jobItemId, string procedureList)
        {
            try
            {
                _savedCertificate = _certificateService.Create(id, certificateTypeId, categoryId, jobItemId, procedureList);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #region GetCertificatesForJobItem

        [Test]
        public void GetCertificatesForJobItem_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _certificateService = CertificateServiceFactory.Create(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IListItemRepository>(),
                MockRepository.GenerateStub<ICertificateRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>());
            GetCertificatesForJobItem(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
        }

        private void GetCertificatesForJobItem(Guid jobItemId)
        {
            try
            {
                _certificateService.GetCertificatesForJobItem(jobItemId);
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
            _certificateService = CertificateServiceFactory.Create(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IListItemRepository>(),
                MockRepository.GenerateStub<ICertificateRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>());
            GetById(Guid.NewGuid());
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
        }

        private void GetById(Guid id)
        {
            try
            {
                _certificateService.GetById(id);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region GetCertificates

        [Test]
        public void GetCertificates_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            _certificateService = CertificateServiceFactory.Create(
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public),
                MockRepository.GenerateStub<IListItemRepository>(),
                MockRepository.GenerateStub<ICertificateRepository>(),
                MockRepository.GenerateStub<IJobItemRepository>());
            GetCertificates();
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
        }

        public void GetCertificates()
        {
            try
            {
                _certificateService.GetCertificates();
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
    }
}