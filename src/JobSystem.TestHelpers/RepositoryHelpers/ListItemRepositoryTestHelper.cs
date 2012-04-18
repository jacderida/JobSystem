using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class ListItemRepositoryTestHelper
	{
		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(Guid currencyId, Guid paymentTermId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			if (currencyId != Guid.Empty)
				listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetCurrency(currencyId));
			else
				listItemRepository.Stub(x => x.GetById(currencyId)).Return(null);
			if (paymentTermId != Guid.Empty)
				listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(GetPaymentTerm(paymentTermId));
			else
				listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(null);
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonCurrency(Guid currencyId, Guid paymentTermId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetNonCurrencyListItem(currencyId));
			listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(GetPaymentTerm(paymentTermId));
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonPaymentTerm(Guid currencyId, Guid paymentTermId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetCurrency(currencyId));
			listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(GetCurrency(paymentTermId));
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsGbpCurrency(Guid currencyId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetCurrency(currencyId));
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsUsdCurrency(Guid currencyId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(currencyId)).Return(GetUsdCurrency(currencyId));
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNull(Guid itemId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(itemId)).Return(null);
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

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsCertificateType(Guid certificateTypeId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(GetHouseCalibrationCertificateType(certificateTypeId));
			return listItemRepository;
		}

		public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonCertificateType(Guid certificateTypeId)
		{
			var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
			listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(GetCurrency(certificateTypeId));
			return listItemRepository;
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

		private static ListItem GetUsdCurrency(Guid currencyId)
		{
			return new ListItem
			{
				Id = currencyId,
				Name = "USD",
				Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Currency", Type = ListItemCategoryType.Currency },
				Type = ListItemType.CurrencyUsd
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

		private static ListItem GetHouseCalibrationCertificateType(Guid certificateTypeId)
		{
			return new ListItem
			{
				Id = certificateTypeId,
				Name = "House",
				Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Certificate Category", Type = ListItemCategoryType.Certificate },
				Type = ListItemType.CertificateTypeHouse
			};
		}

		private static ListItem GetPaymentTerm(Guid paymentTermId)
		{
			return new ListItem
			{
				Id = paymentTermId,
				Name = "30 days",
				Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Certificate Category", Type = ListItemCategoryType.PaymentTerm },
				Type = ListItemType.PaymentTerm30Days
			};
		}
	}
}