using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IDeliveryRepository : IReadWriteRepository<Delivery, Guid>
	{
		int GetDeliveryItemCount(Guid deliveryId);
		IEnumerable<Delivery> GetDeliveries();
	}
}