using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class InstrumentRepository : RepositoryBase<Instrument>, IInstrumentRepository
	{
		public IEnumerable<Instrument> GetInstruments()
		{
			return CurrentSession.Query<Instrument>();
		}

		public IEnumerable<Instrument> SearchByKeyword(string keyword)
		{
			var criteria = CurrentSession.CreateCriteria<Instrument>();
			var keywordCriteria = Restrictions.Disjunction();
			keywordCriteria.Add(Restrictions.InsensitiveLike("Manufacturer", keyword + "%"));
			keywordCriteria.Add(Restrictions.InsensitiveLike("Manufacturer", "% " + keyword + "%"));
			keywordCriteria.Add(Restrictions.InsensitiveLike("ModelNo", keyword + "%"));
			keywordCriteria.Add(Restrictions.InsensitiveLike("ModelNo", "% " + keyword + "%"));
			criteria.Add(keywordCriteria);
			return criteria.List<Instrument>();
		}

		public IEnumerable<Instrument> FindManufacturer(string manufacturer)
		{
			return CurrentSession.Query<Instrument>().Where(i => i.Manufacturer.StartsWith(manufacturer, System.StringComparison.CurrentCultureIgnoreCase));
		}

		public void DeleteAll()
		{
			CurrentSession.CreateSQLQuery("delete from Instruments").ExecuteUpdate();
		}
	}
}