using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IJobRepository : IReadWriteRepository<Job, Guid>
	{
		IEnumerable<Job> GetApprovedJobs();
		IEnumerable<Job> GetPendingJobs();
	}
}