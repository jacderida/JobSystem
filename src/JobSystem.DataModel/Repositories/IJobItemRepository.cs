using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IJobItemRepository : IReadWriteRepository<JobItem, Guid>
	{
		void CreateItemHistory(Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId, Guid workLocationId);
		void CreateItemHistory(ItemHistory itemHistory);
		IEnumerable<JobItem> GetJobItems(Guid jobId);
	}
}