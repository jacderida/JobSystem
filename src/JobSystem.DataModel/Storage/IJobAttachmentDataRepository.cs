using System;

namespace JobSystem.DataModel.Storage
{
	public interface IJobAttachmentDataRepository
	{
		AttachmentData GetById(Guid id);
		void Put(AttachmentData attachmentData);
	}
}