using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class InstrumentRepository : RepositoryBase<Instrument>, IInstrumentRepository
	{
		public IEnumerable<Instrument> GetInstruments()
		{
			return CurrentSession.Query<Instrument>();
		}
	}
}