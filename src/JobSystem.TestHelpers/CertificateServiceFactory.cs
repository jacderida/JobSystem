using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
    public static class CertificateServiceFactory
    {
        public static CertificateService Create(IUserContext userContext, IListItemRepository listItemRepository, ICertificateRepository certificateRepository, IJobItemRepository jobItemRepository)
        {
            return new CertificateService(
                userContext,
                jobItemRepository,
                certificateRepository,
                listItemRepository,
                EntityIdProviderFactory.GetEntityIdProviderFor<Certificate>("CERT2000"),
                MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
        }
    }
}