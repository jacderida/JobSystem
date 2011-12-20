using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Dto;
using NUnit.Framework;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;
using JobSystem.DataModel.Entities;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class CompanyDetailsTests
	{
		private CompanyDetailsService _companyDetailsService;
		private DomainValidationException _domainValidationException;
		private CompanyDetails _savedCompanyDetails;

		#region Setup and Utils

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
		}

		private Address GetAddressDetails()
		{
			return new Address
			{
				Line1 = String.Format("Line1"),
				Line2 = String.Format("Line2"),
				Line3 = String.Format("Line3"),
				Line4 = String.Format("Line4"),
				Line5 = String.Format("Line5")
			};
		}

		private BankDetails GetBankDetails(Guid id)
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

		private ListItem GetCurrency(Guid id)
		{
			return new ListItem
			{
				Id = id,
				Name = "GBP",
				Type = ListItemType.Currency
			};
		}

		private TaxCode GetTaxCode(Guid id)
		{
			return new TaxCode
			{
				Id = id,
				TaxCodeName = "T1",
				Description = "VAT at 20%",
				Rate = 0.20
			};
		}

		private ListItem GetPaymentTerm(Guid id)
		{
			return new ListItem
			{
				Id = id,
				Name = "30 Days",
				Type = ListItemType.PaymentTerm
			};
		}

		#endregion
		#region Create

		[Test]
		public void Create_SuccessfullyCreateCompanyDetails_CompanyDetailsCreated()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			companyDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			companyDetailsRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedCompanyDetails.Id != id);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoCompanyIdSupplied_ArgumentExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.Empty;
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoBankDetailsIdSupplied_ArgumentExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.Empty;
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoTaxCodeIdSupplied_ArgumentExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.Empty;
			var paymentTermId = Guid.NewGuid();
			
			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoCurrencyIdSupplied_ArgumentExceptionThrown()
		{
			var currencyId = Guid.Empty;
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoPaymentTermIdSupplied_ArgumentExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.Empty;

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		public void Create_NameNotSupplied_CompanyDetailsCreated()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, String.Empty, GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.NameRequired));
		}

		[Test]
		public void Create_NameGreaterThan255Characters_CompanyDetailsCreated()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, new string('a', 256), GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.NameTooLarge));
		}

		private void CreateCompanyDetails(
			Guid id, string name, Address addressDetails,
			string telephone, string fax, string email,
			string www, string regNo, string vatRegNo,
			string termsAndConditions, Guid defaultCurrencyId,
			Guid defaultTaxCodeId, Guid defaultPaymentTermsId, Guid defaultBankDetailsId)
		{
			try
			{
				_savedCompanyDetails = _companyDetailsService.Create(
					id, name, addressDetails, telephone, fax, email, www, regNo, vatRegNo, termsAndConditions, defaultCurrencyId, defaultTaxCodeId, defaultPaymentTermsId, defaultBankDetailsId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}