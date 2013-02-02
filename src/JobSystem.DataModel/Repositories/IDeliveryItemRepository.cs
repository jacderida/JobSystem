using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface IDeliveryItemRepository : IReadWriteRepository<DeliveryItem, Guid>
    {
        void CreatePending(PendingDeliveryItem pendingDeliveryItem);
        void DeletePendingDeliveryItem(Guid id);
        bool JobItemHasPendingDeliveryItem(Guid jobItemId);
        DeliveryItem GetDeliveryItemForJobItem(Guid jobItemId);
        PendingDeliveryItem GetPendingDeliveryItemForJobItem(Guid jobItemId);
        PendingDeliveryItem GetPendingDeliveryItem(Guid pendingItemId);
        void UpdatePendingDeliveryItem(PendingDeliveryItem pendingItem);
        IEnumerable<PendingDeliveryItem> GetPendingDeliveryItems();
        IEnumerable<PendingDeliveryItem> GetPendingDeliveryItems(IList<Guid> pendingItemIds);
        IEnumerable<DeliveryItem> GetDeliveryItems(Guid deliveryId);
    }
}