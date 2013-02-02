using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Resources.Instruments;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
    [TestFixture]
    public class InstrumentTests
    {
        private InstrumentService _instrumentService;
        private DomainValidationException _domainValidationException;
        private Instrument _savedInstrument;

        [SetUp]
        public void Setup()
        {
            _instrumentService = InstrumentServiceFactory.Create();
            _domainValidationException = null;
        }

        #region Create

        [Test]
        public void Create_ValidInstrumentDetails_InstrumentCreated()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
            instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            instrumentRepositoryMock.VerifyAllExpectations();
            Assert.AreEqual(id, _savedInstrument.Id);
            Assert.AreEqual(manufacturer, _savedInstrument.Manufacturer);
            Assert.AreEqual(modelNo, _savedInstrument.ModelNo);
            Assert.AreEqual(range, _savedInstrument.Range);
            Assert.AreEqual(description, _savedInstrument.Description);
            Assert.AreEqual(allocatedCalibrationTime, _savedInstrument.AllocatedCalibrationTime);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_InvalidIdSupplied_ArgumentExceptionThrown()
        {
            var id = Guid.Empty;
            var manufacturer = "Druck";
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
        }

        [Test]
        public void Create_ManufacturerGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = new string('A', 51);
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerTooLong));
        }

        [Test]
        public void Create_ManufacturerNotSupplied_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = String.Empty;
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerRequired));
        }

        [Test]
        public void Create_ModelNoGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = new string('A', 51);
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoTooLong));
        }

        [Test]
        public void Create_ModelNoNotSupplied_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = String.Empty;
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoRequired));
        }

        [Test]
        public void Create_RangeGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = "DPI601IS";
            var range = new string('A', 51);
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.RangeTooLong));
        }

        [Test]
        public void Create_DescriptionGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = new string('A', 51);
            var allocatedCalibrationTime = 20;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLong));
        }

        [Test]
        public void Create_AllocatedCalibrationTimeLessThan15_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 14;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidAllocatedCalibrationTime));
        }

        [Test]
        public void Create_AllocatedCalibrationTimeGreaterThan1000_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Druck";
            var modelNo = "DPI601IS";
            var range = "Not Specified";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 1001;

            _instrumentService = InstrumentServiceFactory.Create(MockRepository.GenerateStub<IInstrumentRepository>());
            CreateInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidAllocatedCalibrationTime));
        }

        private void CreateInstrument(Guid id, string manufacturer, string modelNo, string range, string description, int allocatedCalibrationTime)
        {
            try
            {
                _savedInstrument = _instrumentService.Create(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region Edit

        private Instrument GetInstrumentForEdit(Guid instrumentId)
        {
            return new Instrument
            {
                Id = instrumentId,
                Manufacturer = "Druck",
                ModelNo = "DPI601IS",
                Range = "None",
                Description = "Digital Pressure Indicator"
            };
        }

        [Test]
        public void Edit_ValidInstrumentDetails_InstrumentCreated()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = "21";
            var range = "Range";
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
            instrumentRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
            instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            instrumentRepositoryMock.VerifyAllExpectations();
            Assert.AreEqual(manufacturer, _savedInstrument.Manufacturer);
            Assert.AreEqual(modelNo, _savedInstrument.ModelNo);
            Assert.AreEqual(range, _savedInstrument.Range);
            Assert.AreEqual(description, _savedInstrument.Description);
            Assert.AreEqual(allocatedCalibrationTime, _savedInstrument.AllocatedCalibrationTime);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Edit_InvalidInstrumentId_ArgumentExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = "21";
            var range = "Range";
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(null);
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
        }

        [Test]
        public void Edit_ManufacturerGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = new string('A', 51);
            var modelNo = "21";
            var range = "Range";
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerTooLong));
        }

        [Test]
        public void Edit_ManufacturerNotSupplied_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = String.Empty;
            var modelNo = "21";
            var range = "Range";
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerRequired));
        }

        [Test]
        public void Edit_ModelNoGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = new string('a', 51);
            var range = "Range";
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryMock = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoTooLong));
        }

        [Test]
        public void Edit_ModelNoRequired_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = String.Empty;
            var range = "Range";
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoRequired));
        }

        [Test]
        public void Edit_RangeGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = "DPI601IS";
            var range = new string('a', 51);
            var description = "Digital Multimeter";
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.RangeTooLong));
        }

        [Test]
        public void Edit_DescriptionGreaterThan50Characters_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = "DPI601IS";
            var range = "range";
            var description = new string('a', 51);
            var allocatedCalibrationTime = 20;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLong));
        }

        [Test]
        public void Edit_AllocatedCalibrationTimeLessThan15_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = "DPI601IS";
            var range = "range";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 14;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidAllocatedCalibrationTime));
        }

        [Test]
        public void Edit_AllocatedCalibrationTimeGreaterThan1000_DomainValidationExceptionThrown()
        {
            var id = Guid.NewGuid();
            var manufacturer = "Fluke";
            var modelNo = "DPI601IS";
            var range = "range";
            var description = "Digital Pressure Indicator";
            var allocatedCalibrationTime = 1001;

            var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
            instrumentRepositoryStub.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
            _instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryStub);
            EditInstrument(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InvalidAllocatedCalibrationTime));
        }

        private void EditInstrument(
            Guid id, string manufacturer, string modelNo, string range, string description, int allocatedCalibrationTime)
        {
            try
            {
                _savedInstrument = _instrumentService.Edit(id, manufacturer, modelNo, range, description, allocatedCalibrationTime);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }

        #endregion
        #region Get

        [Test]
        public void GetById_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            try
            {
                var id = Guid.NewGuid();
                var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
                _instrumentService = InstrumentServiceFactory.Create(
                    MockRepository.GenerateMock<IInstrumentRepository>(), TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
                _instrumentService.GetById(id);
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
        }

        [Test]
        public void GetInstruments_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
        {
            try
            {
                var id = Guid.NewGuid();
                var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
                _instrumentService = InstrumentServiceFactory.Create(
                    MockRepository.GenerateMock<IInstrumentRepository>(), TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
                _instrumentService.GetInstruments();
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
        }

        #endregion
    }
}