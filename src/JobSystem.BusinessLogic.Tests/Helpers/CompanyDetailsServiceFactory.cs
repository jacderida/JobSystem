using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class CompanyDetailsServiceFactory
	{
		//public static CompanyDetailsService Create()
		//{
		//    return new CompanyDetailsService(
		//        TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
		//        MockRepository.GenerateStub<ICompanyDetailsRepository>(),
		//        MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		//}

		private static BankDetails GetBankDetails(Guid id)
		{
			return new BankDetails
			{
				Id = id,
				Name = "Bank of Scotland",
				AccountNo = "00131183",
				SortCode = "801653",
				Address1 = "High Street",
				Address2 = "Johnstone",
				Address3 = "PA58TE",
				Iban = "blah",
				ShortName = "BoS"
			};
		}

		private static Currency GetCurrency(Guid id)
		{
			return new Currency
			{
				Id = id,
				Name = "GBP"
			};
		}

		private static TaxCode GetTaxCode(Guid id)
		{
			return new TaxCode
			{
				Id = id,
				TaxCodeName = "T1",
				Description = "VAT at 20%",
				Rate = 0.20
			};
		}

		private static PaymentTerm GetPaymentTerm(Guid id)
		{
			return new PaymentTerm
			{
				Id = id,
				Name = "30 Days"
			};
		}

		public static CompanyDetailsService Create(
			ICompanyDetailsRepository companyDetailsRepository,
			IBankDetailsRepository bankDetailsRepository,
			ICurrencyRepository currencyRepository,
			IPaymentTermsRepository paymentTermsRepository,
			ITaxCodeRepository taxCodeRepository)
		{
			return new CompanyDetailsService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				companyDetailsRepository, bankDetailsRepository, currencyRepository,
				paymentTermsRepository, taxCodeRepository, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static CompanyDetailsService CreateWithDefaultsSetup(
			ICompanyDetailsRepository companyDetailsRepository,
			Guid defaultBankDetailsId, Guid defaultCurrencyId, Guid defaultPaymentTermsId, Guid defaultTaxCodeId)
		{
			var bankDetailsRepositoryStub = MockRepository.GenerateStub<IBankDetailsRepository>();
			bankDetailsRepositoryStub.Stub(x => x.GetById(defaultBankDetailsId)).Return(GetBankDetails(defaultBankDetailsId));
			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(defaultCurrencyId)).Return(GetCurrency(defaultCurrencyId));
			var taxCodeRepositoryStub = MockRepository.GenerateStub<ITaxCodeRepository>();
			taxCodeRepositoryStub.Stub(x => x.GetById(defaultTaxCodeId)).Return(GetTaxCode(defaultTaxCodeId));
			var paymentTermsRepositoryStub = MockRepository.GenerateStub<IPaymentTermsRepository>();
			paymentTermsRepositoryStub.Stub(x => x.GetById(defaultTaxCodeId)).Return(GetPaymentTerm(defaultTaxCodeId));
			return new CompanyDetailsService(
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager"),
				companyDetailsRepository, bankDetailsRepositoryStub, currencyRepositoryStub,
				paymentTermsRepositoryStub, taxCodeRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}
	}
}