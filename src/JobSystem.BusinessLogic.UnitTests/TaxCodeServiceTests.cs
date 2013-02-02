using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.TaxCodes;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
    [TestFixture]
    public class TaxCodeServiceTests
    {
        private TaxCode _savedTaxCode;
        private TaxCodeService _taxCodeService;
        private DomainValidationException _domainValidationException;
        private IUserContext _userContext;

        [SetUp]
        public void Setup()
        {
            _savedTaxCode = null;
            _domainValidationException = null;
            _userContext = TestUserContext.Create(
                "graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
        }

        #region Create

        [Test]
        public void Create_ValidDetails_TaxCodeCreate()
        {
            var id = Guid.NewGuid();
            var taxCodeName = "T1";
            var description = "Tax at 20%";
            var rate = 0.2d;

            var taxCodeRepositoryMock = MockRepository.GenerateMock<ITaxCodeRepository>();
            taxCodeRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
            _taxCodeService = new TaxCodeService(
                _userContext, taxCodeRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
            Create(id, taxCodeName, description, rate);
            taxCodeRepositoryMock.VerifyAllExpectations();
            Assert.AreEqual(_savedTaxCode.Id, id);
            Assert.AreEqual(_savedTaxCode.TaxCodeName, taxCodeName);
            Assert.AreEqual(_savedTaxCode.Description, description);
            Assert.AreEqual(_savedTaxCode.Rate, rate);
        }

        [Test]
        public void Create_DuplicateName_ThrowsDomainValidationException()
        {
            var id = Guid.NewGuid();
            var taxCodeName = "T1";
            var description = "Tax at 20%";
            var rate = 0.2d;

            var taxCodeRepositoryStub = MockRepository.GenerateMock<ITaxCodeRepository>();
            taxCodeRepositoryStub.Stub(x => x.GetByName(taxCodeName)).Return(new TaxCode { TaxCodeName = taxCodeName });
            _taxCodeService = new TaxCodeService(
                _userContext, taxCodeRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
            Create(id, taxCodeName, description, rate);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DuplicateName));
        }

        [Test]
        public void Create_TaxCodeTooLarge_ThrowsDomainValidationException()
        {
            var id = Guid.NewGuid();
            var taxCodeName = "T100000000000000000000";
            var description = "Tax at 20%";
            var rate = 0.2d;

            _taxCodeService = new TaxCodeService(
                _userContext, MockRepository.GenerateMock<ITaxCodeRepository>(), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
            Create(id, taxCodeName, description, rate);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.TaxCodeName));
        }

        [Test]
        public void Create_TaxCodeEmpty_ThrowsDomainValidationException()
        {
            var id = Guid.NewGuid();
            var taxCodeName = String.Empty;
            var description = "Tax at 20%";
            var rate = 0.2d;

            _taxCodeService = new TaxCodeService(
                _userContext, MockRepository.GenerateMock<ITaxCodeRepository>(), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
            Create(id, taxCodeName, description, rate);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.TaxCodeEmpty));
        }

        [Test]
        public void Create_DescriptionTooLarge_ThrowsDomainValidationException()
        {
            var id = Guid.NewGuid();
            var taxCodeName = "T1";
            var description = new string('a', 51);
            var rate = 0.2d;

            _taxCodeService = new TaxCodeService(
                _userContext, MockRepository.GenerateMock<ITaxCodeRepository>(), MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
            Create(id, taxCodeName, description, rate);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLarge));
        }

        private void Create(Guid id, string taxCodeName, string description, double rate)
        {
            try
            {
                _savedTaxCode = _taxCodeService.Create(id, taxCodeName, description, rate);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
    }
}