using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface ITestStandardRepository : IReadWriteRepository<TestStandard, Guid>
	{
		IEnumerable<TestStandard> GetTestStandards();
	}
}