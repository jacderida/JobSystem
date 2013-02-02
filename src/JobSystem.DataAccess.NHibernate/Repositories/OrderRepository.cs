using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public int GetApprovedOrdersCount()
        {
            return CurrentSession.Query<Order>().Where(o => o.IsApproved).Count();
        }

        public int GetPendingOrdersCount()
        {
            return CurrentSession.Query<Order>().Where(o => !o.IsApproved).Count();
        }

        public int GetPendingItemsCount()
        {
            return CurrentSession.Query<PendingOrderItem>().Count();
        }

        public IEnumerable<Order> GetOrders()
        {
            return CurrentSession.Query<Order>();
        }
    }
}