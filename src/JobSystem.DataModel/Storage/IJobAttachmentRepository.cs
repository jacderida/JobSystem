using System;

namespace JobSystem.DataModel.Storage
{
	public interface IJobAttachmentRepository
	{
		AttachmentData GetById(Guid id);
		void Put(AttachmentData attachmentData);
	}
}