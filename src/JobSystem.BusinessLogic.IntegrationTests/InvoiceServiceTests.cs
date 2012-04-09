using System;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.TestHelpers.IntegrationHelpers;
using JobSystem.BusinessLogic.Services;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	[TestFixture]
	public class InvoiceServiceTests
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
		public void CreateInvoicesFromPendingItems_ValidPendingInvoiceItems_InvoicesCreated()
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
			var deliveryRepository = new DeliveryRepository();
			var deliveryItemRepository = new DeliveryItemRepository();
			var invoiceRepository = new InvoiceRepository();
			var invoiceItemRepository = new InvoiceItemRepository();

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

			CreateInvoicesFromPendingItemsHelper.CreateContextForPendingItemTests(
				customerId1, customerId2, job1Id, job2Id, job3Id, jobItem1Id, jobItem2Id, jobItem3Id, jobItem4Id, jobItem5Id, jobItem6Id, jobItem7Id, jobItem8Id, jobItem9Id);
			var invoiceItemService = new InvoiceItemService(userContext, companyDetailsRepository, invoiceRepository, invoiceItemRepository, jobItemRepository, quoteItemRepository, dispatcher);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem1Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem2Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem3Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem4Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem5Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem6Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem7Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem8Id);
			invoiceItemService.CreatePending(Guid.NewGuid(), jobItem9Id);


		}
	}
}