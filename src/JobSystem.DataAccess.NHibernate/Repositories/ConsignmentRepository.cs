using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class ConsignmentRepository : RepositoryBase<Consignment>, IConsignmentRepository
	{
		public IEnumerable<Consignment> GetConsignments()
		{
			return CurrentSession.Query<Consignment>();
		}
	}
}