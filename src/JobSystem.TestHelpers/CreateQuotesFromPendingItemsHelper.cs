using System;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers.Context;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public static class CreateQuotesFromPendingItemsHelper
	{
		public static void CreateContextForPendingItemTests(
			Guid job1Id, Guid job2Id, Guid job3Id,
			Guid jobItem1Id, Guid jobItem2Id, Guid jobItem3Id,
			Guid jobItem4Id, Guid jobItem5Id, Guid jobItem6Id, Guid jobItem7Id, Guid jobItem8Id, Guid jobItem9Id)
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

			var instrumentId = Guid.NewGuid();
			var instrumentService = new InstrumentService(userContext, instrumentRepository, dispatcher);
			instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Description");

			var customerId = Guid.NewGuid();
			var customerService = new CustomerService(userContext, customerRepository, dispatcher);
			customerService.Create(customerId, "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());

			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobService = new JobService(userContext, null, jobRepository, listItemRepository, customerRepository, entityIdProvider, dispatcher);
			jobService.CreateJob(job1Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
			jobService.CreateJob(job2Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
			jobService.CreateJob(job3Id, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");
		}
	}
}