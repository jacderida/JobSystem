using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IBankDetailsRepository : IReadWriteRepository<BankDetails, Guid>
	{
		IEnumerable<BankDetails> GetBankDetails();
	}
}