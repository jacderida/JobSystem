using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.Resources.Instruments;
using JobSystem.BusinessLogic.Tests.Context;

namespace JobSystem.BusinessLogic.Tests
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
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, "Druck", "DPI601IS", "None", "Digital Pressure Indicator");
			instrumentRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(id, _savedInstrument.Id);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, new string('A', 51), "DPI601IS", "None", "Digital Pressure Indicator");
		}

		[Test]
		public void Create_ManufacturerGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, new string('A', 51), "DPI601IS", "None", "Digital Pressure Indicator");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerTooLong));
		}

		[Test]
		public void Create_ManufacturerNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, String.Empty, "DPI601IS", "None", "Digital Pressure Indicator");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerRequired));
		}

		[Test]
		public void Create_ModelNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, "Druck", new string('A', 51), "None", "Digital Pressure Indicator");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoTooLong));
		}

		[Test]
		public void Create_ModelNoNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, "Druck", String.Empty, "None", "Digital Pressure Indicator");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoRequired));
		}

		[Test]
		public void Create_RangeGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, "Druck", "DPI601IS", new string('A', 51), "Digital Pressure Indicator");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.RangeTooLong));
		}

		[Test]
		public void Create_DescriptionGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			CreateInstrument(id, "Druck", "DPI601IS", "None", new string('A', 51));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLong));
		}

		private void CreateInstrument(
			Guid id, string manufacturer, string modelNo, string range, string description)
		{
			try
			{
				_savedInstrument = _instrumentService.Create(id, manufacturer, modelNo, range, description);
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
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, "Fluke", "21", "Range", "Digital Multimeter");
			instrumentRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual("Fluke", _savedInstrument.Manufacturer);
			Assert.AreEqual("21", _savedInstrument.ModelNo);
			Assert.AreEqual("Range", _savedInstrument.Range);
			Assert.AreEqual("Digital Multimeter", _savedInstrument.Description);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidInstrumentId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(null);
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, "Fluke", "21", "Range", "Digital Multimeter");
		}

		[Test]
		public void Edit_ManufacturerGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, new string('A', 51), "21", "Range", "Digital Multimeter");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerTooLong));
		}

		[Test]
		public void Edit_ManufacturerNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, String.Empty, "21", "Range", "Digital Multimeter");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ManufacturerRequired));
		}

		[Test]
		public void Edit_ModelNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, "Fluke", new string('a', 51), "Range", "Digital Multimeter");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoTooLong));
		}

		[Test]
		public void Edit_ModelNoRequired_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, "Fluke", String.Empty, "Range", "Digital Multimeter");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ModelNoRequired));
		}

		[Test]
		public void Edit_RangeGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, "Fluke", "21", new string('a', 51), "Digital Multimeter");
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.RangeTooLong));
		}

		[Test]
		public void Edit_DescriptionGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Stub(x => x.GetById(id)).Return(GetInstrumentForEdit(id));
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			EditInstrument(id, "Fluke", "21", "Range", new string('a', 51));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DescriptionTooLong));
		}

		private void EditInstrument(
			Guid id, string manufacturer, string modelNo, string range, string description)
		{
			try
			{
				_savedInstrument = _instrumentService.Edit(id, manufacturer, modelNo, range, description);
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