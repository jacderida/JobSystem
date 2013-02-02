using System;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.IntegrationHelpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.IntegrationTests
{
    [TestFixture]
    public class DeliveryServiceTests
    {
        [SetUp]
        public void Setup()
        {
            NHibernateSession.Current.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            NHibernateSession.Current.Transaction.Rollback();
        }

        [Test]
        public void CreateDeliveriesFromPendingItems_ValidPendingDeliveryItems_DeliveriesCreated()
        {
            var dispatcher = MockRepository.GenerateMock<IQueueDispatcher<IMessage>>();
            var userRepository = new UserAccountRepository();
            var user = userRepository.GetByEmail("admin@intertek.com", false);
            var userContext = new TestUserContext(user);

            var quoteRepository = new QuoteRepository();
            var quoteItemRepository = new QuoteItemRepository();
            var customerRepository = new CustomerRepository();
            var jobRepository = new JobRepository();
            var jobItemRepository = new JobItemRepository();
            var listItemRepository = new ListItemRepository();
            var entityIdProvider = new DirectEntityIdProvider();
            var instrumentRepository = new InstrumentRepository();
            var deliveryRepository = new DeliveryRepository();
            var deliveryItemRepository = new DeliveryItemRepository();

            var customerId1 = Guid.NewGuid();
            var customerId2 = Guid.NewGuid();
            var job1Id = Guid.NewGuid();
            var job2Id = Guid.NewGuid();
            var job3Id = Guid.NewGuid();
            var jobItem1Id = Guid.NewGuid();
            var jobItem2Id = Guid.NewGuid();
            var jobItem3Id = Guid.NewGuid();
            var jobItem4Id = Guid.NewGuid();
            var jobItem5Id = Guid.NewGuid();
            var jobItem6Id = Guid.NewGuid();
            var jobItem7Id = Guid.NewGuid();
            var jobItem8Id = Guid.NewGuid();
            var jobItem9Id = Guid.NewGuid();
            CreateDeliveriesFromPendingItemsHelper.CreateContextForPendingItemTests(
                customerId1, customerId2, job1Id, job2Id, job3Id, jobItem1Id, jobItem2Id, jobItem3Id, jobItem4Id, jobItem5Id, jobItem6Id, jobItem7Id, jobItem8Id, jobItem9Id);

            var deliveryItemService =
                new DeliveryItemService(
                    userContext, deliveryRepository, deliveryItemRepository, jobItemRepository, quoteItemRepository, listItemRepository, customerRepository, dispatcher);
            var deliveryService = new DeliveryService(userContext, deliveryRepository, deliveryItemService, customerRepository, entityIdProvider, dispatcher);
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem1Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem2Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem3Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem4Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem5Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem6Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem7Id, customerId1, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem8Id, customerId2, "some notes");
            deliveryItemService.CreatePending(Guid.NewGuid(), jobItem9Id, customerId2, "some notes");
            deliveryService.CreateDeliveriesFromPendingItems();

            var deliveries = deliveryService.GetDeliveries().ToList();
            Assert.AreEqual(2, deliveries.Count);
            var deliveryItems = deliveryItemService.GetDeliveryItems(deliveries[0].Id).ToList();
            Assert.AreEqual(7, deliveryItems.Count);
        }
    }
}