using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface ICustomerRepository : IReadWriteRepository<Customer, Guid>
	{
		Customer GetByName(string name);
		IEnumerable<Customer> GetCustomers();
	}
}