using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IDeliveryRepository : IReadWriteRepository<Delivery, Guid>
	{
		int GetDeliveryItemCount(Guid deliveryId);
	}
}