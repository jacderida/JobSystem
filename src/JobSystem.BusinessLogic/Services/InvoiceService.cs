using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;
using JobSystem.Resources.Invoices;
using JobSystem.BusinessLogic.Validation.Core;

namespace JobSystem.BusinessLogic.Services
{
	public class InvoiceService : ServiceBase
	{
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly IListItemRepository _listItemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IBankDetailsRepository _bankDetailsRepository;
		private readonly ITaxCodeRepository _taxCodeRepository;

		public InvoiceService(
			IUserContext userContext,
			IInvoiceRepository invoiceRepository,
			IEntityIdProvider entityIdProvider,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IBankDetailsRepository bankDetailsRepository,
			ITaxCodeRepository taxCodeRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(userContext, dispatcher)
		{
			_invoiceRepository = invoiceRepository;
			_entityIdProvider = entityIdProvider;
			_listItemRepository = listItemRepository;
			_customerRepository = customerRepository;
			_bankDetailsRepository = bankDetailsRepository;
			_taxCodeRepository = taxCodeRepository;
		}

		public Invoice Create(Guid id, Guid currencyId, Guid customerId, Guid bankDetailsId, Guid paymentTermId, Guid taxCodeId)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the invoice.");
			var invoice = new Invoice();
			invoice.Id = id;
			invoice.InvoiceNumber = _entityIdProvider.GetIdFor<Invoice>();
			invoice.DateCreated = AppDateTime.GetUtcNow();
			invoice.Currency = GetCurrency(currencyId);
			invoice.Customer = GetCustomer(customerId);
			invoice.BankDetails = GetBankDetails(bankDetailsId);
			invoice.PaymentTerm = GetPaymentTerm(paymentTermId);
			invoice.TaxCode = GetTaxCode(taxCodeId);
			_invoiceRepository.Create(invoice);
			return invoice;
		}

		private ListItem GetCurrency(Guid currencyId)
		{
			var currency = _listItemRepository.GetById(currencyId);
			if (currency == null || currency.Category.Type != ListItemCategoryType.Currency)
				throw new ArgumentException("A valid ID must be supplied for the currency.");
			return currency;
		}

		private Customer GetCustomer(Guid customerId)
		{
			var customer = _customerRepository.GetById(customerId);
			if (customer == null)
				throw new ArgumentException("A valid ID must be supplied for the customer");
			return customer;
		}

		private BankDetails GetBankDetails(Guid bankDetailsId)
		{
			var bankDetails = _bankDetailsRepository.GetById(bankDetailsId);
			if (bankDetails == null)
				throw new ArgumentException("A valid ID must be supplied for the bank details");
			return bankDetails;
		}

		private ListItem GetPaymentTerm(Guid paymentTermId)
		{
			var paymentTerm = _listItemRepository.GetById(paymentTermId);
			if (paymentTerm == null || paymentTerm.Category.Type != ListItemCategoryType.PaymentTerm)
				throw new ArgumentException("A valid ID must be supplied for the payment term");
			return paymentTerm;
		}

		private TaxCode GetTaxCode(Guid taxCodeId)
		{
			var taxCode = _taxCodeRepository.GetById(taxCodeId);
			if (taxCode == null)
				throw new ArgumentException("A valid ID must be supplied for the tax code");
			return taxCode;
		}
	}
}