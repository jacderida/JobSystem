using System;

namespace JobSystem.Storage.Jobs
{
    public interface IJobAttachmentDataRepository
    {
        AttachmentData GetById(Guid id);
        void Put(AttachmentData attachmentData);
    }
}