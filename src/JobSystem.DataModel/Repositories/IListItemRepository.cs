using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IListItemRepository : IReadWriteRepository<ListItem, Guid>
	{
	}
}