using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        public void CreatePending(PendingOrderItem pendingItem)
        {
            CurrentSession.Save(pendingItem);
        }

        public bool JobItemHasPendingOrderItem(Guid jobItemId)
        {
            return CurrentSession.Query<PendingOrderItem>().SingleOrDefault(p => p.JobItem.Id == jobItemId) != null;
        }

        public int GetOrderItemsCount(Guid orderId)
        {
            return CurrentSession.Query<OrderItem>().Where(oi => oi.Order.Id == orderId).Count();
        }

        public IEnumerable<OrderItem> GetOrderItems(Guid orderId)
        {
            return CurrentSession.Query<OrderItem>().Where(oi => oi.Order.Id == orderId);
        }

        public PendingOrderItem GetPendingOrderItem(Guid id)
        {
            return CurrentSession.Query<PendingOrderItem>().SingleOrDefault(p => p.Id == id);
        }

        public PendingOrderItem GetPendingOrderItemForJobItem(Guid jobItemId)
        {
            return CurrentSession.Query<PendingOrderItem>().SingleOrDefault(p => p.JobItem.Id == jobItemId);
        }

        public void UpdatePendingItem(PendingOrderItem pendingItem)
        {
            CurrentSession.Save(pendingItem);
        }

        public void DeletePendingItem(Guid id)
        {
            CurrentSession.Delete(CurrentSession.Get<PendingOrderItem>(id));
        }

        public IEnumerable<OrderItem> GetOrderItemsForJobItem(Guid jobItemId)
        {
            return CurrentSession.Query<OrderItem>().Where(oi => oi.JobItem.Id == jobItemId);
        }

        public IEnumerable<PendingOrderItem> GetPendingOrderItems()
        {
            return CurrentSession.Query<PendingOrderItem>();
        }

        public IEnumerable<PendingOrderItem> GetPendingOrderItems(IList<Guid> pendingItemIds)
        {
            var query = Restrictions.Disjunction();
            foreach (var id in pendingItemIds)
                query.Add(Restrictions.Eq("Id", id));
            var criteria = CurrentSession.CreateCriteria<PendingOrderItem>().Add(query);
            return criteria.List<PendingOrderItem>();
        }
    }
}