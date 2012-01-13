using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IJobItemRepository : IReadWriteRepository<JobItem, Guid>
	{
	}
}