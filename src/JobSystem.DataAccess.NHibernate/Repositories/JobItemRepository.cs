using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class JobItemRepository : RepositoryBase<JobItem>, IJobItemRepository
	{
		public IEnumerable<JobItem> GetJobItems(Guid jobId)
		{
			return CurrentSession.Query<JobItem>().Where(ji => ji.Job.Id == jobId);
		}
	}
}