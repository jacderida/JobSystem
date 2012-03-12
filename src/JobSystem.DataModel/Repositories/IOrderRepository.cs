using System;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;

namespace JobSystem.DataModel.Repositories
{
	public interface IOrderRepository : IReadWriteRepository<Order, Guid>
	{
		IEnumerable<Order> GetOrders();
	}
}