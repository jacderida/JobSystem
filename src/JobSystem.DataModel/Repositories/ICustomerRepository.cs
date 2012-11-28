using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ICustomerRepository : IReadWriteRepository<Customer, Guid>
	{
		Customer GetByName(string name);
		int GetCustomersCount();
		IEnumerable<Customer> GetCustomers();
		IEnumerable<Customer> SearchByKeyword(string keyword);
	}
}