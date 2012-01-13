using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class JobRepository : RepositoryBase<Job>, IJobRepository
	{
		public override Job GetById(Guid id)
		{
			var job = CurrentSession.Get<Job>(id);
			NHibernateUtil.Initialize(job.Customer);
			NHibernateUtil.Initialize(job.Type);
			NHibernateUtil.Initialize(job.CreatedBy);
			return job;
		}

		public int GetJobItemCount(Guid jobId)
		{
			return CurrentSession.Query<JobItem>().Where(ji => ji.Job.Id == jobId).Count();
		}

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