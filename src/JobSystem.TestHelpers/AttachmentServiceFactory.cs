using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Storage.Jobs;
using JobSystem.TestHelpers.Context;

namespace JobSystem.TestHelpers
{
	public static class AttachmentServiceFactory
	{
		public static JobAttachmentService Create(IJobAttachmentDataRepository attachmentDataRepository)
		{
			return Create(
				attachmentDataRepository, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobAttachmentService Create(IJobAttachmentDataRepository attachmentDataRepository, IUserContext userContext)
		{
			return new JobAttachmentService(userContext, attachmentDataRepository);
		}
	}
}