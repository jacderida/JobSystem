using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IOrderRepository : IReadWriteRepository<Order, Guid>
	{
	}
}