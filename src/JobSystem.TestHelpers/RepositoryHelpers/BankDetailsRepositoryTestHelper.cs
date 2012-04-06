using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class BankDetailsRepositoryTestHelper
	{
		public static IBankDetailsRepository GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(Guid bankDetailsId)
		{
			var bankDetailsRepositoryStub = MockRepository.GenerateStub<IBankDetailsRepository>();
			bankDetailsRepositoryStub.Stub(x => x.GetById(bankDetailsId)).Return(GetBankDetails(bankDetailsId));
			return bankDetailsRepositoryStub;
		}

		public static IBankDetailsRepository GetBankDetailsRepository_StubsGetById_ReturnsNull(Guid bankDetailsId)
		{
			var bankDetailsRepositoryStub = MockRepository.GenerateStub<IBankDetailsRepository>();
			bankDetailsRepositoryStub.Stub(x => x.GetById(bankDetailsId)).Return(null);
			return bankDetailsRepositoryStub;
		}

		private static BankDetails GetBankDetails(Guid bankDetailsId)
		{
			return new BankDetails
			{
				Id = Guid.NewGuid(),
				Name = "Bank of Scotland",
				ShortName = "BoS",
				Address1 = "High Street",
				Address2 = "Johnstone",
				AccountNo = "01321321",
				SortCode = "255124",
			};
		}
	}
}