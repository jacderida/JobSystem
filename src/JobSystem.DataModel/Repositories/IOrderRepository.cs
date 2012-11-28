using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IOrderRepository : IReadWriteRepository<Order, Guid>
	{
		int GetApprovedOrdersCount();
		int GetPendingOrdersCount();
		IEnumerable<Order> GetOrders();
	}
}