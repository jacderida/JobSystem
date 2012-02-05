using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{ 
	public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
	{
		public Supplier GetByName(string name)
		{
			return CurrentSession.Query<Supplier>().Where(s => s.Name == name).SingleOrDefault();
		}

		public IEnumerable<Supplier> GetSuppliers()
		{
			return CurrentSession.Query<Supplier>();
		}
	}
}