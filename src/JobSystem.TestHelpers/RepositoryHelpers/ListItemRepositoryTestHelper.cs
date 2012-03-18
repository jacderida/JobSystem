using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class ListItemRepositoryTestHelper
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

		public static IListItemRepository GetListItemRepository_StubsGetByType_ReturnsListItem(ListItemType[] types)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			foreach (var type in types)
				listItemRepositoryStub.Stub(x => x.GetByType(type)).Return(GetByType(type));
			return listItemRepositoryStub;
		}

		private static ListItem GetByType(ListItemType type)
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = type.ToString(),
				Type = type
			};
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