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
		public void Create_NameNotSupplied_DomainValidationExceptionThrown()
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
		public void Create_NameGreaterThan255Characters_DomainValidationExceptionThrown()
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

		[Test]
		public void Create_Address1GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			var addressDetails = GetAddressDetails();
			addressDetails.Line1 = new string('a', 256);
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address2GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			var addressDetails = GetAddressDetails();
			addressDetails.Line2 = new string('a', 256);
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address3GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			var addressDetails = GetAddressDetails();
			addressDetails.Line3 = new string('a', 256);
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address4GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			var addressDetails = GetAddressDetails();
			addressDetails.Line4 = new string('a', 256);
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address5GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			var addressDetails = GetAddressDetails();
			addressDetails.Line5 = new string('a', 256);
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_TelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				new string('a', 56), "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TelephoneTooLarge));
		}

		[Test]
		public void Create_TelephoneNotSupplied_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				String.Empty, "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TelephoneRequired));
		}

		[Test]
		public void Create_FaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", new string('a', 56), "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.FaxTooLarge));
		}

		[Test]
		public void Create_FaxNotSupplied_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", String.Empty, "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.FaxRequired));
		}

		[Test]
		public void Create_EmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", new string('a', 56),
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.EmailTooLarge));
		}

		[Test]
		public void Create_EmailNotSupplied_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 8949229", String.Empty,
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.EmailRequired));
		}

		[Test]
		public void Create_WwwGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				new string('a', 256), "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.WwwTooLarge));
		}

		[Test]
		public void Create_WwwNotSupplied_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 8949229", "info@emis-uk.com",
				String.Empty, "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.WwwRequired));
		}

		[Test]
		public void Create_TermsAndConditionsGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
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
				new string('a', 2001), currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TermsAndConditionsTooLarge));
		}

		[Test]
		public void Create_TermsAndConditionsNotSupplied_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 8949229", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				String.Empty, currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TermsAndConditionsRequired));
		}

		[Test]
		public void Create_RegNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", new string('a', 51), "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.RegNoTooLarge));
		}

		[Test]
		public void Create_VatRegNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var id = Guid.NewGuid();
			CreateCompanyDetails(
				id, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO", new string('a', 51),
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.VatRegNoTooLarge));
		}

		[Test]
		public void Create_InsufficientUserRole_DomainValidationExceptionThrown()
		{
			Assert.IsTrue(false);
			//var currencyId = Guid.NewGuid();
			//var bankDetailsId = Guid.NewGuid();
			//var taxCodeId = Guid.NewGuid();
			//var paymentTermId = Guid.NewGuid();

			//var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			//_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetup(companyDetailsRepositoryMock, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			//var id = Guid.NewGuid();
			//CreateCompanyDetails(
			//    id, "EMIS (UK) Ltd", GetAddressDetails(),
			//    "01224 894494", "01224 894929", "info@emis-uk.com",
			//    "www.emis-uk.com", "REGNO123456", "VATNO123456",
			//    "terms and conditions", currencyId, taxCodeId,
			//    paymentTermId, bankDetailsId);
			//Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.VatRegNoTooLarge));
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
		#region Edit

		[Test]
		public void Edit_ValidCompanyDetails_CompanyDetailsEdited()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			companyDetailsRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			companyDetailsRepositoryMock.VerifyAllExpectations();
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_NoCompanyIdSupplied_ArgumentExceptionThrown()
		{
			var companyDetailsId = Guid.Empty;
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidCompanyIdSupplied_ArgumentExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			companyDetailsRepositoryMock.Stub(x => x.GetById(companyDetailsId)).Return(null);
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidBankDetailsIdSupplied_ArgumentExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.Empty;
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidCurrencyIdSupplied_ArgumentExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.Empty;
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidTaxCodeIdSupplied_ArgumentExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.Empty;
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidPaymentTermsIdSupplied_ArgumentExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.Empty;

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
		}

		[Test]
		public void Edit_NameNotSupplied_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, String.Empty, GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.NameRequired));
		}

		[Test]
		public void Edit_NameGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, new string('a', 256), GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.NameTooLarge));
		}

		[Test]
		public void Edit_Address1GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var addressDetails = GetAddressDetails();
			addressDetails.Line1 = new string('a', 256);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address2GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var addressDetails = GetAddressDetails();
			addressDetails.Line2 = new string('a', 256);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address3GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var addressDetails = GetAddressDetails();
			addressDetails.Line3 = new string('a', 256);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address4GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var addressDetails = GetAddressDetails();
			addressDetails.Line4 = new string('a', 256);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address5GreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			var addressDetails = GetAddressDetails();
			addressDetails.Line5 = new string('a', 256);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", addressDetails,
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_TelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				new string('a', 56), "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TelephoneTooLarge));
		}

		[Test]
		public void Edit_TelephoneNotSupplied_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			CreateCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				String.Empty, "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TelephoneRequired));
		}

		[Test]
		public void Edit_FaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", new string('a', 56), "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.FaxTooLarge));
		}

		[Test]
		public void Edit_FaxNotSupplied_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", String.Empty, "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.FaxRequired));
		}

		[Test]
		public void Edit_EmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", new string('a', 56),
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.EmailTooLarge));
		}

		[Test]
		public void Edit_EmailNotSupplied_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			CreateCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 8949229", String.Empty,
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.EmailRequired));
		}

		[Test]
		public void Edit_WwwGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				new string('a', 256), "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.WwwTooLarge));
		}

		[Test]
		public void Edit_WwwNotSupplied_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 8949229", "info@emis-uk.com",
				String.Empty, "REGNO123456", "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.WwwRequired));
		}

		[Test]
		public void Edit_TermsAndConditionsGreaterThan2000Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				new string('a', 2001), currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TermsAndConditionsTooLarge));
		}

		[Test]
		public void Edit_TermsAndConditionsNotSupplied_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 8949229", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				String.Empty, currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.TermsAndConditionsRequired));
		}

		[Test]
		public void Edit_RegNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			CreateCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", new string('a', 51), "VATNO123456",
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.RegNoTooLarge));
		}

		[Test]
		public void Edit_VatRegNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var companyDetailsId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();

			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			_companyDetailsService = CompanyDetailsServiceFactory.CreateWithDefaultsSetupForEdit(companyDetailsRepositoryMock, companyDetailsId, bankDetailsId, currencyId, paymentTermId, taxCodeId);
			EditCompanyDetails(
				companyDetailsId, "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO", new string('a', 51),
				"terms and conditions", currencyId, taxCodeId,
				paymentTermId, bankDetailsId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.CompanyDetails.Messages.VatRegNoTooLarge));
		}

		private void EditCompanyDetails(
			Guid id, string name, Address addressDetails,
			string telephone, string fax, string email,
			string www, string regNo, string vatRegNo,
			string termsAndConditions, Guid defaultCurrencyId,
			Guid defaultTaxCodeId, Guid defaultPaymentTermsId, Guid defaultBankDetailsId)
		{
			try
			{
				_savedCompanyDetails = _companyDetailsService.Edit(
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