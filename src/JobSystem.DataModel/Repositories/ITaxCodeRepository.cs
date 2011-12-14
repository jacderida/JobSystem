using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ITaxCodeRepository : IReadWriteRepository<TaxCode, Guid>
	{
	}
}