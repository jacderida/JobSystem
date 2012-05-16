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
	public static class CreateOrderFromConsignmentHelper
	{
		public static void CreateContextForCreateOrderFromConsignmentTest(
			Guid jobId, Guid supplierId, Guid consignmentId, Guid jobItem1Id, Guid jobItem2Id, Guid jobItem3Id)
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
			instrumentService.Create(instrumentId, "Druck", "DPI601IS", "None", "Digital Pressure Indicator", 15);

			var customerId = Guid.NewGuid();
			var customerService = new CustomerService(userContext, customerRepository, dispatcher);
			customerService.Create(customerId, "Gael Ltd", String.Empty, new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo(), "Gael Ltd", new Address(), new ContactInfo());

			var supplierService = new SupplierService(userContext, supplierRepository, dispatcher);
			supplierService.Create(supplierId, "Supplier 1", new Address(), new ContactInfo(), new Address(), new ContactInfo());

			var listItemService = new ListItemService(userContext, listItemRepository, dispatcher);
			var jobService = new JobService(userContext, null, jobRepository, listItemRepository, customerRepository, entityIdProvider, dispatcher);
			jobService.CreateJob(jobId, "some instructions", "order no", "advice no", listItemService.GetAllByCategory(ListItemCategoryType.JobType).First().Id, customerId, "notes", "contact");

			var jobItemService = new JobItemService(userContext, jobRepository, jobItemRepository, listItemService, instrumentService, dispatcher);
			jobItemService.CreateJobItem(
				jobId, jobItem1Id, instrumentId, "12345", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				jobId, jobItem2Id, instrumentId, "123456", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobItemService.CreateJobItem(
				jobId, jobItem3Id, instrumentId, "123457", String.Empty,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).First().Id,
				listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).First().Id,
				12, "instructions", String.Empty, false, String.Empty, String.Empty);
			jobService.ApproveJob(jobId);

			var consignmentItemService = new ConsignmentItemService(
				userContext,
				consignmentRepository, consignmentItemRepository, jobItemRepository, new ListItemRepository(), supplierRepository, dispatcher);
			var consignmentService = new ConsignmentService(userContext, consignmentRepository, supplierRepository, new DirectEntityIdProvider(), consignmentItemService, dispatcher);
			consignmentService.Create(consignmentId, supplierId);
			consignmentItemService.Create(Guid.NewGuid(), jobItem1Id, consignmentId, "some consignment instructions");
			consignmentItemService.Create(Guid.NewGuid(), jobItem2Id, consignmentId, "some consignment instructions");
			consignmentItemService.Create(Guid.NewGuid(), jobItem3Id, consignmentId, "some consignment instructions");
		}
	}
}