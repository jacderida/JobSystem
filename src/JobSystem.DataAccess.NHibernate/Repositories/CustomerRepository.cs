using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
	{
		public Customer GetByName(string name)
		{
			return CurrentSession.Query<Customer>().Where(c => c.Name == name).SingleOrDefault();
		}

		public IEnumerable<Customer> GetCustomers()
		{
			return CurrentSession.Query<Customer>().ToList();
		}
	}
}