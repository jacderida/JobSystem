using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Resources.Jobs;

namespace JobSystem.Storage.Jobs
{
    public class JobAttachmentService
    {
        private readonly IUserContext _userContext;
        private readonly IJobAttachmentDataRepository _jobAttachmentDataRepository;

        public JobAttachmentService(
            IUserContext applicationContext,
            IJobAttachmentDataRepository jobAttachmentDataRepository)
        {
            _userContext = applicationContext;
            _jobAttachmentDataRepository = jobAttachmentDataRepository;
        }

        public void Create(AttachmentData attachmentData)
        {
            if (!_userContext.GetCurrentUser().HasRole(UserRole.Member))
                throw new ArgumentException(Messages.InsufficientSecurityClearance);
            if (attachmentData.Id == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the attachment.");
            if (String.IsNullOrEmpty(attachmentData.Filename))
                throw new ArgumentException(Messages.FileNameRequired);
            if (attachmentData.Filename.Length > 2000)
                throw new ArgumentException(Messages.FileNameTooLarge);
            if (String.IsNullOrEmpty(attachmentData.ContentType))
                throw new ArgumentException(Messages.ContentTypeNotSupplied);
            if (attachmentData.Content == null)
                throw new ArgumentException(Messages.ContentNotSupplied);
            _jobAttachmentDataRepository.Put(attachmentData);
        }
    }
}