using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IConsignmentRepository : IReadWriteRepository<Consignment, Guid>
	{
	}
}