using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.DataModel.Repositories;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.RepositoryHelpers;
using JobSystem.Resources.Invoices;

namespace JobSystem.BusinessLogic.UnitTests
{
	public class InvoiceServiceTests
	{
		private InvoiceService _invoiceService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);
		private Invoice _savedInvoice;

		[SetUp]
		public void Setup()
		{
			_savedInvoice = null;
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
		}

		[Test]
		public void Create_ValidInvoiceDetails_InvoiceCreated()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			var invoiceRepositoryMock = MockRepository.GenerateMock<IInvoiceRepository>();
			invoiceRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				invoiceRepositoryMock,
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
			invoiceRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedInvoice.Id);
			Assert.That(_savedInvoice.InvoiceNumber.StartsWith("IR"));
			Assert.AreEqual(_dateCreated, _savedInvoice.DateCreated);
			Assert.IsNotNull(_savedInvoice.Currency);
			Assert.IsNotNull(_savedInvoice.Customer);
			Assert.IsNotNull(_savedInvoice.BankDetails);
			Assert.IsNotNull(_savedInvoice.PaymentTerm);
			Assert.IsNotNull(_savedInvoice.TaxCode);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_CurrencyIdInvalid_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.Empty;
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NonCurrencyListItem_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonCurrency(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_CustomerIdInvalid_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsNull(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidBankDetailsId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsNull(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidPaymentTermId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.Empty;
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NonPaymentTermListItem_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidTaxCodeId_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				_userContext,
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsNull(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var bankDetailsId = Guid.NewGuid();
			var paymentTermId = Guid.NewGuid();
			var taxCodeId = Guid.NewGuid();

			_invoiceService = InvoiceServiceFactory.Create(
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member),
				MockRepository.GenerateStub<IInvoiceRepository>(),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrencyAndPaymentTerm(currencyId, paymentTermId),
				CustomerRepositoryTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				BankDetailsRepositoryTestHelper.GetBankDetailsRepository_StubsGetById_ReturnsBankDetails(bankDetailsId),
				TaxCodeRepositoryTestHelper.GetTaxCodeRepository_StubsGetById_ReturnsTaxCode(taxCodeId));
			Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void Create(Guid id, Guid currencyId, Guid customerId, Guid bankDetailsId, Guid paymentTermId, Guid taxCodeId)
		{
			try
			{
				_savedInvoice = _invoiceService.Create(id, currencyId, customerId, bankDetailsId, paymentTermId, taxCodeId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}