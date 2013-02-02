using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
    public interface IJobRepository : IReadWriteRepository<Job, Guid>
    {
        int GetJobItemCount(Guid jobId);
        IEnumerable<Job> GetApprovedJobs();
        IEnumerable<Job> GetPendingJobs();
        int GetApprovedJobsCount();
    }
}