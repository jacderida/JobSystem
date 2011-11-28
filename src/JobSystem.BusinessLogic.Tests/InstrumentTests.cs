using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class InstrumentTests
	{
		private InstrumentService _instrumentService;
		private DomainValidationException _domainValidationException;

		[SetUp]
		public void Setup()
		{
			_instrumentService = InstrumentServiceFactory.Create();
			_domainValidationException = null;
		}

		public void Create_SuccessfullyCreateInstrument_InstrumentCreated()
		{
			var instrumentRepositoryMock = MockRepository.GenerateMock<IInstrumentRepository>();
			instrumentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_instrumentService = InstrumentServiceFactory.Create(instrumentRepositoryMock);
			var instrument = _instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator");
		}
	}
}