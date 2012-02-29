using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Storage;
using Rhino.Mocks;
using JobSystem.Framework.Messaging;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.TestHelpers.Context;

namespace JobSystem.TestHelpers
{
	public static class AttachmentServiceFactory
	{
		public static AttachmentService Create(IJobAttachmentDataRepository attachmentDataRepository)
		{
			return Create(
				attachmentDataRepository, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static AttachmentService Create(IJobAttachmentDataRepository attachmentDataRepository, IUserContext userContext)
		{
			return new AttachmentService(userContext, attachmentDataRepository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}