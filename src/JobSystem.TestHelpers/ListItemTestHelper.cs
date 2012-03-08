using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public static class ListItemTestHelper
	{
		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsGbpCurrency(Guid currencyId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetCurrency(currencyId));
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNull(Guid currencyId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(null);
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonCurrencyListItem(Guid currencyId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetNonCurrencyListItem(currencyId));
			return listItemRepository;
		}

		private static ListItem GetCurrency(Guid currencyId)
		{
			return new ListItem
			{
				Id = currencyId,
				Name = "GBP",
				Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Currency", Type = ListItemCategoryType.Currency },
				Type = ListItemType.CurrencyGbp
			};
		}

		private static ListItem GetNonCurrencyListItem(Guid currencyId)
		{
			return new ListItem
			{
				Id = currencyId,
				Name = "GBP",
				Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Job Item Category", Type = ListItemCategoryType.JobItemStatus },
				Type = ListItemType.StatusOrdered
			};
		}
	}
}