﻿using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IConsignmentItemRepository : IReadWriteRepository<ConsignmentItem, Guid>
	{
		void CreatePendingItem(PendingConsignmentItem pendingItem);
	}
}