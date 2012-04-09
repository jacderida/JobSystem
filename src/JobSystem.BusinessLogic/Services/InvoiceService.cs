using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Invoices;

namespace JobSystem.BusinessLogic.Services
{
	public class InvoiceService : ServiceBase
	{
		private readonly InvoiceItemService _invoiceItemService;
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly IListItemRepository _listItemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IBankDetailsRepository _bankDetailsRepository;
		private readonly ITaxCodeRepository _taxCodeRepository;
		private readonly ICompanyDetailsRepository _companyDetailsRepository;

		public InvoiceService(
			IUserContext userContext,
			InvoiceItemService invoiceItemService,
			IInvoiceRepository invoiceRepository,
			IEntityIdProvider entityIdProvider,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IBankDetailsRepository bankDetailsRepository,
			ITaxCodeRepository taxCodeRepository,
			ICompanyDetailsRepository companyDetailsRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(userContext, dispatcher)
		{
			_invoiceItemService = invoiceItemService;
			_invoiceRepository = invoiceRepository;
			_entityIdProvider = entityIdProvider;
			_listItemRepository = listItemRepository;
			_customerRepository = customerRepository;
			_bankDetailsRepository = bankDetailsRepository;
			_taxCodeRepository = taxCodeRepository;
			_companyDetailsRepository = companyDetailsRepository;
		}

		public void CreateInvoicesFromPendingItems()
		{
			DoCreateInvoicesFromPendingItems(_invoiceItemService.GetPendingInvoiceItems());
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

		public IEnumerable<Invoice> GetInvoices()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _invoiceRepository.GetInvoices();
		}

		private void DoCreateInvoicesFromPendingItems(IEnumerable<PendingInvoiceItem> pendingItems)
		{
			var company = _companyDetailsRepository.GetCompany();
			var invoiceGroups = pendingItems.GroupBy(g => new { g.JobItem.Job.Id, g.OrderNo });
			foreach (var group in invoiceGroups)
			{
				var i = 0;
				var invoiceId = Guid.NewGuid();
				foreach (var item in group)
				{
					if (i++ == 0)
						Create(invoiceId, company.DefaultCurrency.Id, item.JobItem.Job.Customer.Id, company.DefaultBankDetails.Id, company.DefaultPaymentTerm.Id, company.DefaultTaxCode.Id);
					_invoiceItemService.CreateFromPending(Guid.NewGuid(), invoiceId, item.Description, item.CalibrationPrice, item.RepairPrice, item.PartsPrice, item.CarriagePrice, item.InvestigationPrice, item.JobItem);
					_invoiceItemService.DeletePendingItem(item.Id);
				}
			}
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