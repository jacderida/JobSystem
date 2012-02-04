using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ISupplierRepository : IReadWriteRepository<Supplier, Guid>
	{
		Supplier GetByName(string name);
		IEnumerable<Supplier> GetSuppliers();
	}
}