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
	public class CreateInvoicesFromPendingItemsHelper
	{
		public static void CreateContextForPendingItemTests(
			Guid customerId1, Guid customerId2,
			Guid job1Id, Guid job2Id, Guid job3Id,
			Guid jobItem1Id, Guid jobItem2Id, Guid jobItem3Id,
			Guid jobItem4Id, Guid jobItem5Id, Guid jobItem6Id, Guid jobItem7Id, Guid jobItem8Id, Guid jobItem9Id)
		{
			var dispatcher = MockRepository.GenerateMock<IQueueDispatcher<IMessage>>();
			var userRepository = new UserAccountRepository();
			var user = userRepository.GetByEmail("admin@intertek.com", false);
			var userContext = new TestUserContext(user);

			var companyDetailsRepository = new CompanyDetailsRepository();
			var quoteRepository = new QuoteRepository();
			var quoteItemRepository = new QuoteItemRepository();
			var customerRepository = new CustomerRepository();
			var jobRepository = new JobRepository();
			var jobItemRepository = new JobItemRepository();
			var listItemRepository = new ListItemRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var instrumentRepository = new InstrumentRepository();

			var instrumentId = Guid.NewGuid();
			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Description", 0);

			var customerService = new CustomerService(userContext, customerRepository, dispatcher);
			customerService.Create(customerId1, "Gael Ltd", String.Empty, new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());
			customerService.Create(customerId2, "EMIS (UK) Ltd", String.Empty, new Address(), new ContactInfo(), "EMIS (UK) Ltd", new Address(), new ContactInfo(), "EMIS (UK) Ltd", new Address(), new ContactInfo());

			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobService = new JobService(userContext, null, jobRepository, listItemRepository, customerRepository, entityIdProvider, dispatcher);
			jobService.CreateJob(job1Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId1, "notes", "contact");
			jobService.CreateJob(job2Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId1, "notes", "contact");
			jobService.CreateJob(job3Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId2, "notes", "contact");

			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);

			#region Job 1

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
				job1Id, jobItem4Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobService.ApproveJob(job1Id);

			#endregion
			#region Job2

			jobItemService.CreateJobItem(
				job2Id, jobItem5Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, jobItem6Id, instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job2Id, jobItem7Id, instrumentId, "123457", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobService.ApproveJob(job2Id);

			#endregion
			#region Job 3

			jobItemService.CreateJobItem(
				job3Id, jobItem8Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				job3Id, jobItem9Id, instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobService.ApproveJob(job3Id);

			#endregion

			var quoteItemService = new QuoteItemService(userContext, quoteRepository, quoteItemRepository, jobItemRepository, listItemRepository, customerRepository, dispatcher);
			var quoteService = new QuoteService(userContext, quoteRepository, customerRepository, entityIdProvider, listItemRepository, quoteItemService, companyDetailsRepository, dispatcher);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem1Id, 25, 35, 45, 25, 56, "calibrated", 30, false, "CALORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem3Id, 45, 65, 35, 22, 87, "calibrated", 30, false, "CALORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem2Id, 45, 60, 30, 26, 56, "repaired", 30, false, "REPAIRORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem4Id, 45, 65, 35, 22, 87, "repaired", 30, false, "REPAIRORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem5Id, 25, 35, 45, 25, 56, "calibrated", 30, false, "CALORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem6Id, 25, 35, 45, 25, 56, "calibrated", 30, false, "CALORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId1, jobItem7Id, 25, 35, 45, 25, 56, "calibrated", 30, false, "CALORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId2, jobItem8Id, 25, 35, 45, 25, 56, "repaired", 30, false, "REPAIRORDER", String.Empty);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId2, jobItem9Id, 25, 35, 45, 25, 56, "repaired", 30, false, "REPAIRORDER", String.Empty);
			quoteService.CreateQuotesFromPendingItems();

			foreach (var quote in quoteService.GetQuotes())
				foreach (var quoteItem in quoteItemService.GetQuoteItems(quote.Id))
					quoteItemService.Accept(quoteItem.Id);
		}
	}
}