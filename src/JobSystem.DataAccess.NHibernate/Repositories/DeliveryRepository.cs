using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class DeliveryRepository : RepositoryBase<Delivery>, IDeliveryRepository
    {
        public int GetDeliveryItemCount(Guid deliveryId)
        {
            return CurrentSession.Query<DeliveryItem>().Where(di => di.Delivery.Id == deliveryId).Count();
        }

        public int GetDeliveriesCount()
        {
            return CurrentSession.Query<Delivery>().Count();
        }

        public IEnumerable<Delivery> GetDeliveries()
        {
            return CurrentSession.Query<Delivery>();
        }
    }
}