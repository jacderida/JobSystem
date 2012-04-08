using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class QuoteItemRepositoryTestHelper
	{
		public static IQuoteItemRepository GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(Guid jobItemId, QuoteItem quoteItem)
		{
			var quoteItemRepository = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepository.Stub(x => x.GetQuoteItemForJobItem(jobItemId)).Return(quoteItem);
			return quoteItemRepository;
		}

		public static IQuoteItemRepository GetQuoteItemRepository_StubsGetById_ReturnsQuoteItem(Guid quoteItemId)
		{
			var quoteItemRepository = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepository.Stub(x => x.GetById(quoteItemId)).Return(GetQuoteItem(quoteItemId));
			return quoteItemRepository;
		}

		public static IQuoteItemRepository GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsQuoteItem(Guid jobItemId)
		{
			var quoteItemRepository = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepository.Stub(x => x.GetQuoteItemForJobItem(jobItemId)).Return(GetQuoteItemForJobItem(jobItemId));
			return quoteItemRepository;
		}

		public static IQuoteItemRepository GetQuoteItemRepository_StubsGetQuoteItemForJobItem_ReturnsNull(Guid jobItemId)
		{
			var quoteItemRepository = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepository.Stub(x => x.GetQuoteItemForJobItem(jobItemId)).Return(null);
			return quoteItemRepository;
		}

		public static IQuoteItemRepository GetQuoteItemRepository_StubsJobItemHasPendingQuoteItem_ReturnsTrue(Guid jobItemId)
		{
			var quoteItemRepository = MockRepository.GenerateStub<IQuoteItemRepository>();
			quoteItemRepository.Stub(x => x.JobItemHasPendingQuoteItem(jobItemId)).Return(true);
			return quoteItemRepository;
		}

		private static QuoteItem GetQuoteItemForJobItem(Guid jobItemId)
		{
			return new QuoteItem
			{
				Id = Guid.NewGuid(),
				ItemNo = 1,
				Calibration = 50,
				Labour = 40,
				Carriage = 25,
				Parts = 30,
				Investigation = 25,
				Report = "calibrated ok",
				Status = new ListItem { Id = Guid.NewGuid(), Type = ListItemType.StatusQuotedPrepared, Name = "Quote Prepared" },
				Days = 30,
				Quote = new Quote { Id = Guid.NewGuid() },
				JobItem = new JobItem { Id = jobItemId }
			};
		}

		private static QuoteItem GetQuoteItem(Guid quoteItemId)
		{
			return new QuoteItem
			{
				Id = Guid.NewGuid(),
				ItemNo = 1,
				Calibration = 50,
				Labour = 40,
				Carriage = 25,
				Parts = 30,
				Investigation = 25,
				Report = "calibrated ok",
				Status = new ListItem { Id = Guid.NewGuid(), Type = ListItemType.StatusQuotedPrepared, Name = "Quote Prepared" },
				Days = 30,
				Quote = new Quote { Id = Guid.NewGuid() }
			};
		}
	}
}