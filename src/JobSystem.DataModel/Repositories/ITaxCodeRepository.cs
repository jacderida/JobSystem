using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface ITaxCodeRepository : IReadWriteRepository<TaxCode, Guid>
	{
		TaxCode GetByName(string name);
		IEnumerable<TaxCode> GetTaxCodes();
	}
}