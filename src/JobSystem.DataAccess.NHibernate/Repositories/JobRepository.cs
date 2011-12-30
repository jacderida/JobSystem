using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class JobRepository : RepositoryBase<Job>, IJobRepository
	{
		public IEnumerable<Job> GetApprovedJobs()
		{
			return CurrentSession.Query<Job>().Where(j => !j.IsPending);
		}

		public IEnumerable<Job> GetPendingJobs()
		{
			return CurrentSession.Query<Job>().Where(j => j.IsPending);
		}
	}
}