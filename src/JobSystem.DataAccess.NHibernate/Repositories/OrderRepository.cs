using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
		public IEnumerable<Order> GetOrders()
		{
			return CurrentSession.Query<Order>();
		}
	}
}