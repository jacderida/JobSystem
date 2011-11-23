using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ICustomerRepository : IReadWriteRepository<Customer, Guid>
	{
		Customer GetByName(string name);
	}
}