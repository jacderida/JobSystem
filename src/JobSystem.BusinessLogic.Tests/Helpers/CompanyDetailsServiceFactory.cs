using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class CompanyDetailsServiceFactory
	{
		public static CompanyDetailsService CreateWithDefaultsSetup(
			ICompanyDetailsRepository companyDetailsRepository, Guid defaultBankDetailsId, Guid defaultCurrencyId, Guid defaultPaymentTermsId, Guid defaultTaxCodeId)
		{
			return CreateWithDefaultsSetup(companyDetailsRepository, defaultBankDetailsId, defaultCurrencyId, defaultPaymentTermsId, defaultTaxCodeId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Admin));
		}

		public static CompanyDetailsService CreateWithDefaultsSetup(
			ICompanyDetailsRepository companyDetailsRepository, Guid defaultBankDetailsId, Guid defaultCurrencyId, Guid defaultPaymentTermsId, Guid defaultTaxCodeId, IUserContext userContext)
		{
			var bankDetailsRepositoryStub = GetBankDetailsRepository(defaultBankDetailsId);
			var taxCodeRepositoryStub = GetTaxCodeRepository(defaultTaxCodeId);
			var listItemsRepositoryStub = GetDefaultListItemRepository(defaultCurrencyId, defaultPaymentTermsId);
			return new CompanyDetailsService(
				userContext,
				companyDetailsRepository, bankDetailsRepositoryStub, listItemsRepositoryStub, taxCodeRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		public static CompanyDetailsService CreateWithDefaultsSetupForEdit(
			ICompanyDetailsRepository companyDetailsRepository, Guid defaultBankDetailsId, Guid defaultCurrencyId, Guid defaultPaymentTermsId, Guid defaultTaxCodeId)
		{
			return CreateWithDefaultsSetupForEdit(companyDetailsRepository,  defaultBankDetailsId, defaultCurrencyId, defaultPaymentTermsId, defaultTaxCodeId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Admin));
		}

		public static CompanyDetailsService CreateWithDefaultsSetupForEdit(
			ICompanyDetailsRepository companyDetailsRepository, Guid defaultBankDetailsId, Guid defaultCurrencyId, Guid defaultPaymentTermsId, Guid defaultTaxCodeId, IUserContext userContext)
		{
			companyDetailsRepository.Stub(x => x.GetCompany()).Return(GetCompanyDetails(defaultBankDetailsId, defaultTaxCodeId, defaultCurrencyId, defaultPaymentTermsId));
			var bankDetailsRepositoryStub = GetBankDetailsRepository(defaultBankDetailsId);
			var taxCodeRepositoryStub = GetTaxCodeRepository(defaultTaxCodeId);
			var listItemsRepositoryStub = GetDefaultListItemRepository(defaultCurrencyId, defaultPaymentTermsId);
			return new CompanyDetailsService(
				userContext,
				companyDetailsRepository, bankDetailsRepositoryStub, listItemsRepositoryStub, taxCodeRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		private static IListItemRepository GetDefaultListItemRepository(Guid defaultCurrencyId, Guid defaultPaymentTermsId)
		{
			var listItemsRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (defaultCurrencyId != Guid.Empty)
				listItemsRepositoryStub.Stub(x => x.GetById(defaultCurrencyId)).Return(GetCurrency(defaultCurrencyId));
			else
				listItemsRepositoryStub.Stub(x => x.GetById(defaultCurrencyId)).Return(null);
			if (defaultPaymentTermsId != Guid.Empty)
				listItemsRepositoryStub.Stub(x => x.GetById(defaultPaymentTermsId)).Return(GetPaymentTerm(defaultPaymentTermsId));
			else
				listItemsRepositoryStub.Stub(x => x.GetById(defaultPaymentTermsId)).Return(null);
			return listItemsRepositoryStub;
		}

		private static IBankDetailsRepository GetBankDetailsRepository(Guid defaultBankDetailsId)
		{
			var bankDetailsRepositoryStub = MockRepository.GenerateStub<IBankDetailsRepository>();
			if (defaultBankDetailsId != Guid.Empty)
				bankDetailsRepositoryStub.Stub(x => x.GetById(defaultBankDetailsId)).Return(GetBankDetails(defaultBankDetailsId));
			else
				bankDetailsRepositoryStub.Stub(x => x.GetById(defaultBankDetailsId)).Return(null);
			return bankDetailsRepositoryStub;
		}

		private static ITaxCodeRepository GetTaxCodeRepository(Guid defaultTaxCodeId)
		{
			var taxCodeRepositoryStub = MockRepository.GenerateStub<ITaxCodeRepository>();
			if (defaultTaxCodeId != Guid.Empty)
				taxCodeRepositoryStub.Stub(x => x.GetById(defaultTaxCodeId)).Return(GetTaxCode(defaultTaxCodeId));
			else
				taxCodeRepositoryStub.Stub(x => x.GetById(defaultTaxCodeId)).Return(null);
			return taxCodeRepositoryStub;
		}

		private static CompanyDetails GetCompanyDetails(Guid defaultBankDetailsId, Guid defaultTaxCodeId, Guid defaultCurrencyId, Guid defaultPaymentTermId)
		{
			return new CompanyDetails
			{
				Id = Guid.NewGuid(),
				Name = "EMIS (UK) Ltd",
				Address1 = "CEB",
				Address2 = "Greenwell Road",
				Address3 = "Aberdeen",
				Address4 = "AB12 3AX",
				Address5 = "UK",
				Email = "info@emis-uk.com",
				Www = "www.emis-uk.com",
				Telephone = "01224 894494",
				Fax = "01224 894929",
				RegNo = "REGNO",
				VatRegNo = "VatRegNo",
				TermsAndConditions = "terms and conditions",
				DefaultBankDetails = GetBankDetails(defaultBankDetailsId),
				DefaultCurrency = GetCurrency(defaultCurrencyId),
				DefaultTaxCode = GetTaxCode(defaultTaxCodeId),
				DefaultPaymentTerm = GetPaymentTerm(defaultPaymentTermId)
			};
		}

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

		private static ListItem GetCurrency(Guid id)
		{
			return new ListItem
			{
				Id = id,
				Name = "GBP",
				Type = ListItemType.Currency
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

		private static ListItem GetPaymentTerm(Guid id)
		{
			return new ListItem
			{
				Id = id,
				Name = "30 Days",
				Type = ListItemType.PaymentTerm
			};
		}
	}
}