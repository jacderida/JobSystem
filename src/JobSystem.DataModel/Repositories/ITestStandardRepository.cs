﻿using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ITestStandardRepository : IReadWriteRepository<TestStandard, Guid>
	{
	}
}