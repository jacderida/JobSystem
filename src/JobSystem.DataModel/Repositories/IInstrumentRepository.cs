using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IInstrumentRepository : IReadWriteRepository<Instrument, Guid>
	{
		IEnumerable<Instrument> GetInstruments();
		IEnumerable<Instrument> SearchByKeyword(string keyword);
		IEnumerable<string> SearchManufacturerByKeyword(string keyword);
		IEnumerable<string> SearchModelNoByKeywordFilterByManufacturer(string keyword, string manufacturer);
	}
}