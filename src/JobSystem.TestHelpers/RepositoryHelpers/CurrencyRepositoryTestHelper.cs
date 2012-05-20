using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public class CurrencyRepositoryTestHelper
	{
		public static ICurrencyRepository GetCurrencyRepository_StubsGetById_ReturnsNull(Guid currencyId)
		{
			var currencyRepository = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepository.Stub(x => x.GetById(currencyId)).Return(null);
			return currencyRepository;
		}

		public static ICurrencyRepository GetCurrencyRepository_StubsGetById_ReturnsGbpCurrency(Guid currencyId)
		{
			var currencyRepository = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepository.Stub(x => x.GetById(currencyId)).Return(GetCurrency(currencyId));
			return currencyRepository;
		}

		public static ICurrencyRepository GetCurrencyRepository_StubsGetById_ReturnsUsdCurrency(Guid currencyId)
		{
			var currencyRepository = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepository.Stub(x => x.GetById(currencyId)).Return(GetUsdCurrency(currencyId));
			return currencyRepository;
		}

		private static Currency GetCurrency(Guid currencyId)
		{
			return new Currency
			{
				Id = currencyId,
				Name = "GBP",
				DisplayMessage = "All prices in GBP"
			};
		}

		private static Currency GetUsdCurrency(Guid currencyId)
		{
			return new Currency
			{
				Id = currencyId,
				Name = "USD",
				DisplayMessage = "All prices in USD"
			};
		}
	}
}