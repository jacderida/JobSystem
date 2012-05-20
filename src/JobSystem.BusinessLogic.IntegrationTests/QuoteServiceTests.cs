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
	public class QuoteServiceTests
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
		public void CreateQuotesFromPendingItems_ValidPendingQuoteItems_QuotesCreated()
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
			var currencyRepository = new CurrencyRepository();
			var entityIdProvider = new DirectEntityIdProvider();
			var instrumentRepository = new InstrumentRepository();

			var customerId = Guid.NewGuid();
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
			CreateQuotesFromPendingItemsHelper.CreateContextForPendingItemTests(
				customerId, job1Id, job2Id, job3Id, jobItem1Id, jobItem2Id, jobItem3Id, jobItem4Id, jobItem5Id, jobItem6Id, jobItem7Id, jobItem8Id, jobItem9Id);

			var quoteItemService = new QuoteItemService(
				userContext, quoteRepository, quoteItemRepository, jobItemRepository, listItemRepository, customerRepository, dispatcher);
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem1Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1000", "AD1000");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem2Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1001", "AD1001");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem3Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1000", "AD1000");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem4Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1001", "AD1001");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem5Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1000", "AD1000");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem6Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1000", "AD1000");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem7Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1000", "AD1000");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem8Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1200", "AD1200");
			quoteItemService.CreatePending(Guid.NewGuid(), customerId, jobItem9Id, 85, 40, 39, 25, 12, "report", 30, false, "PO1200", "AD1200");
			
			var quoteService = new QuoteService(
				userContext, quoteRepository, customerRepository, entityIdProvider, currencyRepository, quoteItemService, new CompanyDetailsRepository(), dispatcher);
			quoteService.CreateQuotesFromPendingItems();

			var quotes = quoteService.GetQuotes().OrderBy(q => q.QuoteNumber).ToList();
			Assert.AreEqual(4, quotes.Count);
			Assert.AreEqual("1", quotes[0].QuoteNumber);
			Assert.AreEqual("PO1000", quotes[0].OrderNumber);
			Assert.AreEqual("PO1001", quotes[1].OrderNumber);
			Assert.AreEqual("PO1000", quotes[2].OrderNumber);
			Assert.AreEqual("PO1200", quotes[3].OrderNumber);

			var quoteItems = quoteItemService.GetQuoteItems(quotes[0].Id).ToList();
			Assert.AreEqual(2, quoteItems.Count);
			Assert.AreEqual(jobItem1Id, quoteItems[0].JobItem.Id);
			Assert.AreEqual(jobItem3Id, quoteItems[1].JobItem.Id);

			quoteItems = quoteItemService.GetQuoteItems(quotes[1].Id).ToList();
			Assert.AreEqual(2, quoteItems.Count);
			Assert.AreEqual(jobItem2Id, quoteItems[0].JobItem.Id);
			Assert.AreEqual(jobItem4Id, quoteItems[1].JobItem.Id);

			quoteItems = quoteItemService.GetQuoteItems(quotes[2].Id).ToList();
			Assert.AreEqual(3, quoteItems.Count);
			Assert.AreEqual(jobItem5Id, quoteItems[0].JobItem.Id);
			Assert.AreEqual(jobItem6Id, quoteItems[1].JobItem.Id);
			Assert.AreEqual(jobItem7Id, quoteItems[2].JobItem.Id);

			quoteItems = quoteItemService.GetQuoteItems(quotes[3].Id).ToList();
			Assert.AreEqual(2, quoteItems.Count);
			Assert.AreEqual(jobItem8Id, quoteItems[0].JobItem.Id);
			Assert.AreEqual(jobItem9Id, quoteItems[1].JobItem.Id);
		}
	}
}