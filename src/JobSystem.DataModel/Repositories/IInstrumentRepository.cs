using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IInstrumentRepository : IReadWriteRepository<Instrument, Guid>
	{
	}
}