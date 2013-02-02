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
    public static class CertificateRegisterSearchTestHelper
    {
        public static void CreateContext(Guid testStandardId, Guid jobItemId)
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
            instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Description", 15);

            var customerId = Guid.NewGuid();
            var customerService = new CustomerService(userContext, customerRepository, dispatcher);
            customerService.Create(customerId, "Gael Ltd", String.Empty, new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());

            var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
            var jobService = new JobService(userContext, null, jobRepository, listItemRepository, customerRepository, entityIdProvider, dispatcher);
            var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);

            var jobId = Guid.NewGuid();
            jobService.CreateJob(jobId, "Some instructions", "PO1000", "AD1000", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "some notes", "job contact");
            jobItemService.CreateJobItem(
                jobId, jobItemId, instrumentId, "12345", String.Empty,
                listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
                listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
                12, "instructions", String.Empty, false, String.Empty, String.Empty);
        }
    }
}