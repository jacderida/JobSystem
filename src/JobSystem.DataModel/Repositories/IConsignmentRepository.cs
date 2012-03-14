using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IConsignmentRepository : IReadWriteRepository<Consignment, Guid>
	{
		int GetConsignmentItemCount(Guid consignmentId);
		IEnumerable<Consignment> GetConsignments();
		IEnumerable<ConsignmentItem> GetConsignmentItems(Guid consignmentId);
	}
}