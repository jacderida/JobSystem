using System;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.TestHelpers;
using NUnit.Framework;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	[TestFixture]
	public class InstrumentSearchTests
	{
		private InstrumentService _instrumentService;
		private InstrumentRepository _instrumentRepository = new InstrumentRepository();

		[SetUp]
		public void Setup()
		{
			NHibernateSession.Current.BeginTransaction();
		}

		[TearDown]
		public void TearDown()
		{
			NHibernateSession.Current.Transaction.Rollback();
		}

		[Test]
		public void SearchByKeyword_ManufacturerExactMatch_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("Druck");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ManufacturerExactMatchCaseInsensitive_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("dRucK");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ManufacturerContains_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("Dru");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ManufacturerContainsCaseInsensitive_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("dRu");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ModelNoExactMatch_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("DPI601is");
			Assert.AreEqual(1, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ModelNoExactMatchCaseInsensitive_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("dpi601IS");
			Assert.AreEqual(1, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ModelNoContains_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("DPI");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ModelNoContainsCaseInsensitive_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "FLK100", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("dpi");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchByKeyword_ManufacturerOrModelNoContains_2Results()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Bird", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "DruckDPI601", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchByKeyword("Druck");
			Assert.AreEqual(2, instruments.ToList().Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchManufacturerByKeyword_ManufacturerContains_1Result()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Bird", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI701IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "DruckDPI601", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchManufacturerByKeyword("dru").ToList();
			Assert.AreEqual(1, instruments.Count);
			_instrumentRepository.DeleteAll();
		}

		[Test]
		public void SearchModelNoByKeywordFilterByManufacturer_ModelNoContains_1Result()
		{
			_instrumentService = InstrumentServiceFactory.CreateForSearch(_instrumentRepository);
			_instrumentService.Create(Guid.NewGuid(), "Bird", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 0);
			_instrumentService.Create(Guid.NewGuid(), "Fluke", "DruckDPI601", "None", "Digital Pressure Indicator", 0);

			var instruments = _instrumentService.SearchModelNoByKeywordFilterByManufacturer("dpi601", "Druck").ToList();
			Assert.AreEqual(1, instruments.Count);
			_instrumentRepository.DeleteAll();
		}
	}
}