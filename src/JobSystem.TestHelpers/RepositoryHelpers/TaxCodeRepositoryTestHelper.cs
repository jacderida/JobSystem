using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class TaxCodeRepositoryTestHelper
	{
		public static ITaxCodeRepository GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(Guid taxCodeId)
		{
			var taxCodeRepositoryStub = MockRepository.GenerateStub<ITaxCodeRepository>();
			taxCodeRepositoryStub.Stub(x => x.GetById(taxCodeId)).Return(GetTaxCode(taxCodeId));
			return taxCodeRepositoryStub;
		}

		public static ITaxCodeRepository GetTaxCodeRepository_StubsGetById_ReturnsNull(Guid taxCodeId)
		{
			var taxCodeRepositoryStub = MockRepository.GenerateStub<ITaxCodeRepository>();
			taxCodeRepositoryStub.Stub(x => x.GetById(taxCodeId)).Return(null);
			return taxCodeRepositoryStub;
		}

		private static TaxCode GetTaxCode(Guid taxCodeId)
		{
			return new TaxCode
			{
				Id = Guid.NewGuid(),
				TaxCodeName = "T1",
				Description = "blah",
				Rate = 0.02d
			};
		}
	}
}