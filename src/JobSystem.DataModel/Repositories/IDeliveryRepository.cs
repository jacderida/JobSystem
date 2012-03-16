using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IDeliveryRepository : IReadWriteRepository<Delivery, Guid>
	{
	}
}