using System;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	[TestFixture]
	public class ConsignmentTests
	{
		private ConsignmentService _consignmentService;

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
		public void CreateConsignmentsFromPendingItems_ValidPendingConsignmentItems_ConsignmentsCreated()
		{
			var dispatcher = MockRepository.GenerateMock<IQueueDispatcher<IMessage>>();
			var userRepository = new UserAccountRepository();
			var user = userRepository.GetByEmail("admin@intertek.com", false);
			var userContext = new TestUserContext(user);

			var consignmentRepository = new ConsignmentRepository();
			var consignmentItemRepository = new ConsignmentItemRepository();
			var supplierRepository = new SupplierRepository();
			var jobRepository = new JobRepository();
			var jobItemRepository = new JobItemRepository();
			var listItemRepository = new ListItemRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var instrumentRepository = new InstrumentRepository();

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
			CreateConsignmentsFromPendingItemsHelper.CreateContextForPendingItemTests(
				job1Id, job2Id, job3Id, supplier1Id, supplier2Id, supplier3Id,
				jobItem1Id, jobItem2Id, jobItem3Id, jobItem4Id, jobItem5Id, jobItem6Id, jobItem7Id);

			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);
			var consignmentItemService = new ConsignmentItemService(
				userContext,
				consignmentRepository, consignmentItemRepository, jobItemRepository, new ListItemRepository(), supplierRepository, dispatcher);
			_consignmentService = new ConsignmentService(userContext, consignmentRepository, supplierRepository, new DirectEntityIdProvider(), consignmentItemService, dispatcher);
			var jobItems = jobItemService.GetJobItems(job1Id).OrderBy(ji => ji.ItemNo).ToList();
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[0].Id, supplier1Id, "for cal");
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[1].Id, supplier2Id, "for repair");
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[2].Id, supplier1Id, "for cal");
			jobItems = jobItemService.GetJobItems(job2Id).OrderBy(ji => ji.ItemNo).ToList();
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[0].Id, supplier1Id, "for cal");
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[1].Id, supplier2Id, "for repair");
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[2].Id, supplier3Id, "for cal");
			jobItems = jobItemService.GetJobItems(job3Id).OrderBy(ji => ji.ItemNo).ToList();
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[0].Id, supplier3Id, "for cal");

			var consignmentService = new ConsignmentService(userContext, consignmentRepository, supplierRepository, entityIdProvider, consignmentItemService, dispatcher);
			consignmentService.CreateConsignmentsFromPendingItems();

			var consignments = consignmentService.GetConsignments().OrderBy(c => c.ConsignmentNo).ToList();
			Assert.AreEqual(3, consignments.Count);
			Assert.AreEqual("1", consignments[0].ConsignmentNo);
			Assert.AreEqual(supplier1Id, consignments[0].Supplier.Id);
			Assert.AreEqual("2", consignments[1].ConsignmentNo);
			Assert.AreEqual(supplier2Id, consignments[1].Supplier.Id);
			Assert.AreEqual("3", consignments[2].ConsignmentNo);
			Assert.AreEqual(supplier3Id, consignments[2].Supplier.Id);

			var consignmentItems = consignmentItemService.GetConsignmentItems(consignments[0].Id).OrderBy(ci => ci.ItemNo).ToList();
			Assert.AreEqual(3, consignmentItems.Count);
			Assert.AreEqual(jobItem1Id, consignmentItems[0].JobItem.Id);	// JR2000/1
			Assert.AreEqual(jobItem3Id, consignmentItems[1].JobItem.Id);	// JR2000/3
			Assert.AreEqual(jobItem4Id, consignmentItems[2].JobItem.Id);	// JR2001/1

			consignmentItems = consignmentItemService.GetConsignmentItems(consignments[1].Id).OrderBy(ci => ci.ItemNo).ToList();
			Assert.AreEqual(2, consignmentItems.Count);
			Assert.AreEqual(jobItem2Id, consignmentItems[0].JobItem.Id);	// JR2000/2
			Assert.AreEqual(jobItem5Id, consignmentItems[1].JobItem.Id);	// JR2001/2

			consignmentItems = consignmentItemService.GetConsignmentItems(consignments[2].Id).OrderBy(ci => ci.ItemNo).ToList();
			Assert.AreEqual(2, consignmentItems.Count);
			Assert.AreEqual(jobItem6Id, consignmentItems[0].JobItem.Id);	// JR2001/3
			Assert.AreEqual(jobItem7Id, consignmentItems[1].JobItem.Id);	// JR2002/1

			Assert.AreEqual(0, consignmentItemService.GetPendingItems().Count());
		}

		[Test]
		public void CreateConsignmentsFromPendingItems_SelectedValidPendingConsignmentItems_ConsignmentsCreated()
		{
			var dispatcher = MockRepository.GenerateMock<IQueueDispatcher<IMessage>>();
			var userRepository = new UserAccountRepository();
			var user = userRepository.GetByEmail("admin@intertek.com", false);
			var userContext = new TestUserContext(user);

			var consignmentRepository = new ConsignmentRepository();
			var consignmentItemRepository = new ConsignmentItemRepository();
			var supplierRepository = new SupplierRepository();
			var jobRepository = new JobRepository();
			var jobItemRepository = new JobItemRepository();
			var listItemRepository = new ListItemRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var instrumentRepository = new InstrumentRepository();

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
			CreateConsignmentsFromPendingItemsHelper.CreateContextForPendingItemTests(
				job1Id, job2Id, job3Id, supplier1Id, supplier2Id, supplier3Id,
				jobItem1Id, jobItem2Id, jobItem3Id, jobItem4Id, jobItem5Id, jobItem6Id, jobItem7Id);

			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);
			var consignmentItemService = new ConsignmentItemService(
				userContext,
				consignmentRepository, consignmentItemRepository, jobItemRepository, new ListItemRepository(), supplierRepository, dispatcher);
			_consignmentService = new ConsignmentService(userContext, consignmentRepository, supplierRepository, new DirectEntityIdProvider(), consignmentItemService, dispatcher);
			var jobItems = jobItemService.GetJobItems(job1Id).OrderBy(ji => ji.ItemNo).ToList();

			var pendingItem1Id = Guid.NewGuid();
			var pendingItem2Id = Guid.NewGuid();
			var pendingItem3Id = Guid.NewGuid();
			var pendingItem4Id = Guid.NewGuid();
			consignmentItemService.CreatePending(pendingItem1Id, jobItems[0].Id, supplier1Id, "for cal");
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[1].Id, supplier2Id, "for repair");
			consignmentItemService.CreatePending(pendingItem2Id, jobItems[2].Id, supplier1Id, "for cal");
			jobItems = jobItemService.GetJobItems(job2Id).OrderBy(ji => ji.ItemNo).ToList();
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[0].Id, supplier1Id, "for cal");
			consignmentItemService.CreatePending(pendingItem3Id, jobItems[1].Id, supplier2Id, "for repair");
			consignmentItemService.CreatePending(Guid.NewGuid(), jobItems[2].Id, supplier3Id, "for cal");
			jobItems = jobItemService.GetJobItems(job3Id).OrderBy(ji => ji.ItemNo).ToList();
			consignmentItemService.CreatePending(pendingItem4Id, jobItems[0].Id, supplier3Id, "for cal");

			var consignmentService = new ConsignmentService(userContext, consignmentRepository, supplierRepository, entityIdProvider, consignmentItemService, dispatcher);
			consignmentService.CreateConsignmentsFromPendingItems(new Guid[] { pendingItem1Id, pendingItem2Id, pendingItem3Id, pendingItem4Id });

			var consignments = consignmentService.GetConsignments().OrderBy(c => c.ConsignmentNo).ToList();
			Assert.AreEqual(3, consignments.Count);
			Assert.AreEqual("1", consignments[0].ConsignmentNo);
			Assert.AreEqual(supplier1Id, consignments[0].Supplier.Id);
			Assert.AreEqual("2", consignments[1].ConsignmentNo);
			Assert.AreEqual(supplier2Id, consignments[1].Supplier.Id);
			Assert.AreEqual("3", consignments[2].ConsignmentNo);
			Assert.AreEqual(supplier3Id, consignments[2].Supplier.Id);

			var consignmentItems = consignmentItemService.GetConsignmentItems(consignments[0].Id).OrderBy(ci => ci.ItemNo).ToList();
			Assert.AreEqual(2, consignmentItems.Count);
			Assert.AreEqual(jobItem1Id, consignmentItems[0].JobItem.Id);	// JR2000/1
			Assert.AreEqual(jobItem3Id, consignmentItems[1].JobItem.Id);	// JR2000/3

			consignmentItems = consignmentItemService.GetConsignmentItems(consignments[1].Id).OrderBy(ci => ci.ItemNo).ToList();
			Assert.AreEqual(1, consignmentItems.Count);
			Assert.AreEqual(jobItem5Id, consignmentItems[0].JobItem.Id);	// JR2001/2

			consignmentItems = consignmentItemService.GetConsignmentItems(consignments[2].Id).OrderBy(ci => ci.ItemNo).ToList();
			Assert.AreEqual(1, consignmentItems.Count);
			Assert.AreEqual(jobItem7Id, consignmentItems[0].JobItem.Id);	// JR2002/1

			Assert.AreEqual(3, consignmentItemService.GetPendingItems().Count());
		}
	}
}