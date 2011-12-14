using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IBankDetailsRepository : IReadWriteRepository<BankDetails, Guid>
	{
	}
}