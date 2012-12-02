using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IDeliveryRepository : IReadWriteRepository<Delivery, Guid>
	{
		int GetDeliveryItemCount(Guid deliveryId);
		int GetDeliveriesCount();
		IEnumerable<Delivery> GetDeliveries();
	}
}