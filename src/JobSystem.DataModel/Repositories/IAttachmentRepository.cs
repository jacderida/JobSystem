using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IAttachmentRepository : IReadWriteRepository<Attachment, Guid>
	{
	}
}