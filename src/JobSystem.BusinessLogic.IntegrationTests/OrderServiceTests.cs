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
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	[TestFixture]
	public class OrderServiceTests
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
		public void CreateOrdersFromPendingItems_ValidPendingOrderItems_OrdersCreated()
		{
			var dispatcher = MockRepository.GenerateMock<IQueueDispatcher<IMessage>>();
			var userRepository = new UserAccountRepository();
			var user = userRepository.GetByEmail("admin@intertek.com", false);
			var userContext = new TestUserContext(user);

			var orderRepository = new OrderRepository();
			var orderItemRepository = new OrderItemRepository();
			var supplierRepository = new SupplierRepository();
			var jobRepository = new JobRepository();
			var jobItemRepository = new JobItemRepository();
			var listItemRepository = new ListItemRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var instrumentRepository = new InstrumentRepository();
			var companyDetailsRepository = new CompanyDetailsRepository();
			var consignmentRepository = new ConsignmentRepository();

			var supplier1Id = Guid.NewGuid();
			var supplier2Id = Guid.NewGuid();
			var supplier3Id = Guid.NewGuid();
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
			CreateOrdersFromPendingItemsHelper.CreateContextForPendingItemTests(
				job1Id, job2Id, job3Id, supplier1Id, supplier2Id, supplier3Id,
				jobItem1Id, jobItem2Id, jobItem3Id, jobItem4Id, jobItem5Id, jobItem6Id, jobItem7Id);
			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);
			var orderItemService = new OrderItemService(
				userContext, orderRepository, orderItemRepository, supplierRepository, jobItemRepository, listItemRepository, dispatcher);
			
			orderItemService.CreatePending(Guid.NewGuid(), supplier1Id, "some description", 1, "partno", "instructions", 30, jobItem1Id, 29.99m);
			orderItemService.CreatePending(Guid.NewGuid(), supplier2Id, "some description", 1, "partno", "instructions", 30, jobItem2Id, 29.99m);
			orderItemService.CreatePending(Guid.NewGuid(), supplier1Id, "some description", 1, "partno", "instructions", 30, jobItem3Id, 29.99m);

			orderItemService.CreatePending(Guid.NewGuid(), supplier1Id, "some description", 1, "partno", "instructions", 30, jobItem4Id, 29.99m);
			orderItemService.CreatePending(Guid.NewGuid(), supplier2Id, "some description", 1, "partno", "instructions", 30, jobItem5Id, 29.99m);
			orderItemService.CreatePending(Guid.NewGuid(), supplier3Id, "some description", 1, "partno", "instructions", 30, jobItem6Id, 29.99m);

			orderItemService.CreatePending(Guid.NewGuid(), supplier3Id, "some description", 1, "partno", "instructions", 30, jobItem7Id, 29.99m);

			var orderService = new OrderService(
				userContext, orderRepository, consignmentRepository, supplierRepository, listItemRepository, entityIdProvider, orderItemService, companyDetailsRepository, dispatcher);
			orderService.CreateOrdersFromPendingItems();

			var orders = orderService.GetOrders().OrderBy(o => o.OrderNo).ToList();
			Assert.AreEqual(3, orders.Count);
			Assert.AreEqual("1", orders[0].OrderNo);
			Assert.AreEqual(supplier1Id, orders[0].Supplier.Id);
			Assert.AreEqual("2", orders[1].OrderNo);
			Assert.AreEqual(supplier2Id, orders[1].Supplier.Id);
			Assert.AreEqual("3", orders[2].OrderNo);
			Assert.AreEqual(supplier3Id, orders[2].Supplier.Id);

			var orderItems = orderItemService.GetOrderItems(orders[0].Id).OrderBy(oi => oi.ItemNo).ToList();
			Assert.AreEqual(3, orderItems.Count);
			Assert.AreEqual(jobItem1Id, orderItems[0].JobItem.Id);	// JR2000/1
			Assert.AreEqual(jobItem3Id, orderItems[1].JobItem.Id);	// JR2000/3
			Assert.AreEqual(jobItem4Id, orderItems[2].JobItem.Id);	// JR2001/1

			orderItems = orderItemService.GetOrderItems(orders[1].Id).OrderBy(oi => oi.ItemNo).ToList();
			Assert.AreEqual(2, orderItems.Count);
			Assert.AreEqual(jobItem2Id, orderItems[0].JobItem.Id);	// JR2000/2
			Assert.AreEqual(jobItem5Id, orderItems[1].JobItem.Id);	// JR2001/2

			orderItems = orderItemService.GetOrderItems(orders[1].Id).OrderBy(oi => oi.ItemNo).ToList();
			Assert.AreEqual(2, orderItems.Count);
			Assert.AreEqual(jobItem6Id, orderItems[0].JobItem.Id);	// JR2001/3
			Assert.AreEqual(jobItem7Id, orderItems[1].JobItem.Id);	// JR2002/1

			Assert.AreEqual(0, orderItemService.GetPendingOrderItems().Count());
		}

		[Test]
		public void CreateOrderFromConsignment_ValidConsignment_OrderCreated()
		{
			var dispatcher = MockRepository.GenerateMock<IQueueDispatcher<IMessage>>();
			var userRepository = new UserAccountRepository();
			var user = userRepository.GetByEmail("admin@intertek.com", false);
			var userContext = new TestUserContext(user);

			var orderRepository = new OrderRepository();
			var orderItemRepository = new OrderItemRepository();
			var supplierRepository = new SupplierRepository();
			var listItemRepository = new ListItemRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var jobItemRepository = new JobItemRepository();
			var companyDetailsRepository = new CompanyDetailsRepository();
			var consignmentRepository = new ConsignmentRepository();

			var supplierId = Guid.NewGuid();
			var jobId = Guid.NewGuid();
			var jobItem1Id = Guid.NewGuid();
			var jobItem2Id = Guid.NewGuid();
			var jobItem3Id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();

			CreateOrderFromConsignmentHelper.CreateContextForCreateOrderFromConsignmentTest(jobId, supplierId, consignmentId, jobItem1Id, jobItem2Id, jobItem3Id);
			//var consignmentItemService = new ConsignmentItemService(
			//    userContext, new ConsignmentRepository(), new ConsignmentItemRepository(), jobItemRepository, listItemRepository, supplierRepository, dispatcher);
			//var items = consignmentItemService.GetConsignmentItems(consignmentId);
			var orderItemService = new OrderItemService(userContext, orderRepository, orderItemRepository, supplierRepository, jobItemRepository, listItemRepository, dispatcher);
			var orderService = new OrderService(
				userContext, orderRepository, consignmentRepository, supplierRepository, listItemRepository, entityIdProvider,
				orderItemService, companyDetailsRepository, dispatcher);
			var orderId = orderService.CreateOrderFromConsignment(consignmentId);
			var order = orderService.GetById(orderId);
			Assert.AreEqual(supplierId, order.Supplier.Id);
			Assert.AreEqual(3, order.Items.Count);
			var orderItems = orderItemService.GetOrderItems(orderId).OrderBy(o => o.ItemNo).ToList();
			Assert.AreEqual(ListItemType.StatusItemWithSubContractor, orderItems[0].JobItem.Status.Type);
			Assert.AreEqual("Druck, DPI601IS, None, Digital Pressure Indicator", orderItems[0].Description);
		}
	}
}