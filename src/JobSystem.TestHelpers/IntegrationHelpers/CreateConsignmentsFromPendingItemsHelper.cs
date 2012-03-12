using System;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers.Context;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.IntegrationHelpers
{
	public static class CreateConsignmentsFromPendingItemsHelper
	{
		public static void CreateContextForPendingItemTests(
			Guid job1Id, Guid job2Id, Guid job3Id, Guid supplier1Id, Guid supplier2Id, Guid supplier3Id,
			Guid jobItem1Id, Guid jobItem2Id, Guid jobItem3Id, Guid jobItem4Id, Guid jobItem5Id, Guid jobItem6Id, Guid jobItem7Id)
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

			var instrumentId = Guid.NewGuid();
			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Description");

			var customerId = Guid.NewGuid();
			var customerService = new CustomerService(userContext, customerRepository, dispatcher);
			customerService.Create(customerId, "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());
			
			var supplierService = new SupplierService(userContext, supplierRepository, dispatcher);
			supplierService.Create(supplier1Id, "Supplier 1", new Address(), new ContactInfo(), new Address(), new ContactInfo());
			supplierService.Create(supplier2Id, "Supplier 2", new Address(), new ContactInfo(), new Address(), new ContactInfo());
			supplierService.Create(supplier3Id, "Supplier 3", new Address(), new ContactInfo(), new Address(), new ContactInfo());

			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobService = new JobService(userContext, null, jobRepository, listItemRepository, customerRepository, entityIdProvider, dispatcher);
			jobService.CreateJob(job1Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
			jobService.CreateJob(job2Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
			jobService.CreateJob(job3Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");

			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);
			jobItemService.CreateJobItem(
				job1Id, jobItem1Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job1Id, jobItem2Id, instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job1Id, jobItem3Id, instrumentId, "123457", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, jobItem4Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, jobItem5Id, instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, jobItem6Id, instrumentId, "1234567", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job3Id, jobItem7Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobService.ApproveJob(job1Id);
			jobService.ApproveJob(job2Id);
			jobService.ApproveJob(job3Id);
		}
	}
}