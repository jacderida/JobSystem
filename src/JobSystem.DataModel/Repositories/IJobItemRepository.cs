﻿using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IJobItemRepository : IReadWriteRepository<JobItem, Guid>
	{
		IEnumerable<JobItem> GetJobItems(Guid jobId);
	}
}