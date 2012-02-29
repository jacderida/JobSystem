using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Storage;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Jobs;

namespace JobSystem.BusinessLogic.Services
{
	public class AttachmentService : ServiceBase
	{
		private readonly IJobAttachmentDataRepository _jobAttachmentDataRepository;

		public AttachmentService(
			IUserContext applicationContext,
			IJobAttachmentDataRepository jobAttachmentDataRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_jobAttachmentDataRepository = jobAttachmentDataRepository;
		}

		public void Create(AttachmentData attachmentData)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			if (attachmentData.Id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the attachment.");
			if (String.IsNullOrEmpty(attachmentData.Filename))
				throw new DomainValidationException(Messages.FileNameRequired, "Filename");
			if (attachmentData.Filename.Length > 2000)
				throw new DomainValidationException(Messages.FileNameTooLarge, "Filename");
			if (String.IsNullOrEmpty(attachmentData.ContentType))
				throw new DomainValidationException(Messages.ContentTypeNotSupplied, "ContentType");
			if (attachmentData.Content == null)
				throw new DomainValidationException(Messages.ContentNotSupplied, "Content");
			_jobAttachmentDataRepository.Put(attachmentData);
		}
	}
}