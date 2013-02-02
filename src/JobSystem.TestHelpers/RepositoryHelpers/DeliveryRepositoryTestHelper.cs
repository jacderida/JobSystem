using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
    public static class DeliveryRepositoryTestHelper
    {
        public static IDeliveryRepository GetDeliveryRepository_StubsGetByIdForDeliveryWith0Items_ReturnsDelivery(Guid deliveryId)
        {
            var deliveryRepository = MockRepository.GenerateStub<IDeliveryRepository>();
            deliveryRepository.Stub(x => x.GetById(deliveryId)).Return(GetDelivery(deliveryId));
            deliveryRepository.Stub(x => x.GetDeliveryItemCount(deliveryId)).Return(0);
            return deliveryRepository;
        }

        public static IDeliveryRepository GetDeliveryRepository_StubsGetByIdForDeliveryWith1Item_ReturnsDelivery(Guid deliveryId)
        {
            var deliveryRepository = MockRepository.GenerateStub<IDeliveryRepository>();
            deliveryRepository.Stub(x => x.GetById(deliveryId)).Return(GetDelivery(deliveryId));
            deliveryRepository.Stub(x => x.GetDeliveryItemCount(deliveryId)).Return(1);
            return deliveryRepository;
        }

        public static IDeliveryRepository GetDeliveryRepository_StubsGetById_ReturnsNull(Guid deliveryId)
        {
            var deliveryRepository = MockRepository.GenerateStub<IDeliveryRepository>();
            deliveryRepository.Stub(x => x.GetById(deliveryId)).Return(null);
            return deliveryRepository;
        }

        private static Delivery GetDelivery(Guid deliveryId)
        {
            return new Delivery
            {
                Id = deliveryId,
                DeliveryNoteNumber = "DR2000",
                Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
                DateCreated = DateTime.Now,
                CreatedBy = new UserAccount { Id = Guid.NewGuid() },
                Fao = "Graham Robertson"
            };
        }
    }
}