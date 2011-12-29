using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IJobRepository : IReadWriteRepository<Job, Guid>
	{
	}
}