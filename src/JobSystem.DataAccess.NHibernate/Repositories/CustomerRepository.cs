using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public Customer GetByName(string name)
        {
            return CurrentSession.Query<Customer>().Where(c => c.Name == name).SingleOrDefault();
        }

        public int GetCustomersCount()
        {
            return CurrentSession.Query<Customer>().Count();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return CurrentSession.Query<Customer>();
        }

        public IEnumerable<Customer> SearchByKeyword(string keyword)
        {
            var criteria = CurrentSession.CreateCriteria<Customer>();
            var keywordCriteria = Restrictions.Disjunction();
            keywordCriteria.Add(Restrictions.InsensitiveLike("Name", keyword + "%"));
            keywordCriteria.Add(Restrictions.InsensitiveLike("Name", "% " + keyword + "%"));
            criteria.Add(keywordCriteria);
            return criteria.List<Customer>();
        }
    }
}