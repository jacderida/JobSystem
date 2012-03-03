using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;
using System.Collections.Generic;

namespace JobSystem.TestHelpers
{
	public static class QuoteItemServiceTestHelper
	{
		public static QuoteItemService CreateQuoteItemService(
			IUserContext userContext, IQuoteRepository quoteRepository, IQuoteItemRepository quoteItemRepository, IJobItemRepository jobItemRepository, IListItemRepository listItemRepository,
			ICustomerRepository customerRepository)
		{
			return new QuoteItemService(
				userContext, quoteRepository, quoteItemRepository, jobItemRepository, listItemRepository, customerRepository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static JobItemService CreateJobItemService(IUserContext userContext, IJobItemRepository jobItemRepository)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				MockRepository.GenerateStub<IJobRepository>(),
				jobItemRepository,
				new ListItemService(userContext, MockRepository.GenerateStub<IListItemRepository>(), dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static IQuoteRepository GetQuoteRepository_StubsGetById_ReturnsQuoteWith0Items(Guid quoteId)
		{
			var quoteRepositoryStub = MockRepository.GenerateStub<IQuoteRepository>();
			quoteRepositoryStub.Stub(x => x.GetById(quoteId)).Return(GetQuote(quoteId));
			return quoteRepositoryStub;
		}

		public static IQuoteRepository GetQuoteRepository_StubsGetById_ReturnsQuoteWith1Item(Guid quoteId)
		{
			var quoteRepositoryStub = MockRepository.GenerateStub<IQuoteRepository>();
			quoteRepositoryStub.Stub(x => x.GetById(quoteId)).Return(GetQuoteWith1Item(quoteId));
			return quoteRepositoryStub;
		}

		public static IQuoteRepository GetQuoteRepository_StubsGetById_ReturnsNull(Guid quoteId)
		{
			var quoteRepositoryStub = MockRepository.GenerateStub<IQuoteRepository>();
			quoteRepositoryStub.Stub(x => x.GetById(quoteId)).Return(null);
			return quoteRepositoryStub;
		}

		public static IJobItemRepository GetJobItemRepository_StubsGetById_ReturnsNull(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(null);
			return jobItemRepositoryStub;
		}

		public static IJobItemRepository GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(GetJobItem(jobItemId, true));
			return jobItemRepositoryStub;
		}

		public static IJobItemRepository GetJobItemRepository_StubsGetById_ReturnsJobItem(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(GetJobItem(jobItemId, false));
			return jobItemRepositoryStub;
		}

		public static IListItemRepository GetListItemRepository_StubsGetByTypeCalls_ReturnsStatusQuotePreparedAndLocationQuoted()
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.StatusQuotedPrepared)).Return(GetQuotePreparedListItem());
			listItemRepositoryStub.Stub(x => x.GetByType(ListItemType.WorkLocationQuoted)).Return(GetQuotedLocationListItem());
			return listItemRepositoryStub;
		}

		private static Quote GetQuote(Guid quoteId)
		{
			return new Quote
			{
				Id = quoteId,
				CreatedBy = new UserAccount
				{
					Id = Guid.NewGuid(),
					EmailAddress = "chris.oneil@gmail.com",
					Name = "Chris O'Neil",
					JobTitle = "Development Engineer",
					Roles = UserRole.Manager | UserRole.Member,
					PasswordHash = "passwordhash",
					PasswordSalt = "passwordsalt"
				},
				QuoteNumber = "QR2000",
				OrderNumber = "PO1000",
				AdviceNumber = "AD1000",
				DateCreated = DateTime.Now,
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" }
			};
		}

		private static Quote GetQuoteWith1Item(Guid quoteId)
		{
			return new Quote
			{
				Id = quoteId,
				CreatedBy = new UserAccount
				{
					Id = Guid.NewGuid(),
					EmailAddress = "chris.oneil@gmail.com",
					Name = "Chris O'Neil",
					JobTitle = "Development Engineer",
					Roles = UserRole.Manager | UserRole.Member,
					PasswordHash = "passwordhash",
					PasswordSalt = "passwordsalt"
				},
				QuoteNumber = "QR2000",
				OrderNumber = "PO1000",
				AdviceNumber = "AD1000",
				DateCreated = DateTime.Now,
				Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
				QuoteItems = new List<QuoteItem>()
				{
					new QuoteItem
					{
						Id = Guid.NewGuid(),
						ItemNo = 1,
					}
				}
			};
		}

		private static ListItem GetQuotePreparedListItem()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Quote Prepared",
				Type = ListItemType.StatusQuotedPrepared,
				Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.JobItemStatus, Name = "Job Item Status" }
			};
		}

		private static ListItem GetQuotedLocationListItem()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Quoted",
				Type = ListItemType.WorkLocationQuoted,
				Category = new ListItemCategory { Id = Guid.NewGuid(), Type = ListItemCategoryType.JobItemLocation, Name = "Job Item Location" }
			};
		}

		private static JobItem GetJobItem(Guid jobItemId, bool isPending)
		{
			var createdBy = new UserAccount
			{
				Id = Guid.NewGuid(),
				EmailAddress = "chris.oneil@gmail.com",
				Name = "Chris O'Neil",
				JobTitle = "Development Engineer",
				Roles = UserRole.Manager | UserRole.Member,
				PasswordHash = "passwordhash",
				PasswordSalt = "passwordsalt"
			};
			return new JobItem
			{
				Id = jobItemId,
				Job = new Job
				{
					Id = Guid.NewGuid(),
					JobNo = "JR2000",
					CreatedBy = createdBy,
					OrderNo = "ORDER12345",
					DateCreated = DateTime.UtcNow,
					Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
					IsPending = isPending
				},
				ItemNo = 1,
				SerialNo = "12345",
				Instrument = new Instrument
				{
					Id = Guid.NewGuid(),
					Manufacturer = "Druck",
					ModelNo = "DPI601IS",
					Range = "None",
					Description = "Digital Pressure Indicator"
				},
				CalPeriod = 12,
				Created = DateTime.UtcNow,
				CreatedUser = createdBy,
			};
		}
	}
}