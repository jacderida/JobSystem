using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
    public static class DeliveryItemServiceFactory
    {
        public static DeliveryItemService Create(
            IUserContext userContext,
            IDeliveryRepository deliveryRepository,
            IDeliveryItemRepository deliveryItemRepository,
            IJobItemRepository jobItemRepository,
            IQuoteItemRepository quoteItemRepository,
            IListItemRepository listItemRepository,
            ICustomerRepository customerRepository)
        {
            return new DeliveryItemService(
                userContext,
                deliveryRepository,
                deliveryItemRepository,
                jobItemRepository,
                quoteItemRepository,
                listItemRepository,
                customerRepository,
                MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
        }
    }
}