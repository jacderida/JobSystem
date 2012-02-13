using System;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	[TestFixture]
	public class ConsignmentTests : IntegrationTest
	{
		private ConsignmentService _consignmentService;

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
			var customerRepository = new CustomerRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var instrumentRepository = new InstrumentRepository();

			NHibernateSession.Current.BeginTransaction();
			var instrumentId = Guid.NewGuid();
			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Description");

			var customerId = Guid.NewGuid();
			var customerService = new CustomerService(userContext, customerRepository, dispatcher);
			customerService.Create(customerId, "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());

			var supplier1Id = Guid.NewGuid();
			var supplier2Id = Guid.NewGuid();
			var supplier3Id = Guid.NewGuid();
			var supplierService = new SupplierService(userContext, supplierRepository, dispatcher);
			supplierService.Create(supplier1Id, "Supplier 1", new Address(), new ContactInfo(), new Address(), new ContactInfo());
			supplierService.Create(supplier2Id, "Supplier 2", new Address(), new ContactInfo(), new Address(), new ContactInfo());
			supplierService.Create(supplier3Id, "Supplier 3", new Address(), new ContactInfo(), new Address(), new ContactInfo());

			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var job1Id = Guid.NewGuid();
			var job2Id = Guid.NewGuid();
			var job3Id = Guid.NewGuid();
			var jobService = new JobService(userContext, jobRepository, listItemRepository, customerRepository, entityIdProvider, dispatcher);
			jobService.CreateJob(job1Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
			jobService.CreateJob(job2Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
			jobService.CreateJob(job3Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");

			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);
			jobItemService.CreateJobItem(
				job1Id, Guid.NewGuid(), instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job1Id, Guid.NewGuid(), instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job1Id, Guid.NewGuid(), instrumentId, "123457", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, Guid.NewGuid(), instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, Guid.NewGuid(), instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, Guid.NewGuid(), instrumentId, "1234567", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job3Id, Guid.NewGuid(), instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobService.ApproveJob(job1Id);
			jobService.ApproveJob(job2Id);
			jobService.ApproveJob(job3Id);

			NHibernateSession.Current.Transaction.Begin();
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
			NHibernateSession.Current.Transaction.Commit();

			NHibernateSession.Current.Transaction.Begin();
			var consignmentService = new ConsignmentService(userContext, consignmentRepository, supplierRepository, entityIdProvider, consignmentItemService, dispatcher);
			consignmentService.CreateConsignmentsFromPendingItems();
			NHibernateSession.Current.Transaction.Commit();

			NHibernateSession.Current.Transaction.Begin();
			foreach (var consignment in consignmentService.GetConsignments())
			{
				Console.WriteLine("Consignment {0}", consignment.ConsignmentNo);
				foreach (var item in consignmentItemService.GetConsignmentItems(consignment.Id).OrderBy(ci => ci.ItemNo))
					Console.WriteLine("Item {0}: {1}/{2}, {3}", item.ItemNo, item.JobItem.Job.JobNo, item.JobItem.ItemNo, consignment.Supplier.Name);
			}
		}
	}
}