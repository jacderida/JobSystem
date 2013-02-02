using System;
using JobSystem.Storage.Jobs;
using JobSystem.Framework.Configuration;

namespace JobSystem.Storage.Providers.S3
{
    public class S3JobAttachmentDataRepository : S3StorageProvider, IJobAttachmentDataRepository
    {
        public static readonly string JobStoragePath = "jobattachments";

        public S3JobAttachmentDataRepository(IHostNameProvider hostNameProvider) : base(hostNameProvider)
        {
        }

        public AttachmentData GetById(Guid id)
        {
            var key = String.Format("{0}/{1}", JobStoragePath, id);
            return ToAttachmentData(Get(key), id);
        }

        public void Put(AttachmentData attachmentData)
        {
            var path = String.Format("{0}/{1}", JobStoragePath, attachmentData.Id);
            Put(path, attachmentData.Filename, attachmentData.ContentType, attachmentData.Content);
        }
    }
}