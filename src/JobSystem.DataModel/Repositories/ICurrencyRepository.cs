﻿using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ICurrencyRepository : IReadWriteRepository<Currency, Guid>
	{
	}
}