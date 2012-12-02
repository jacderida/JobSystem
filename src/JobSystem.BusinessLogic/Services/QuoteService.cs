using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Quotes;

namespace JobSystem.BusinessLogic.Services
{
	public class QuoteService : ServiceBase
	{
		private readonly IQuoteRepository _quoteRepository;
		private readonly ICurrencyRepository _currencyRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly QuoteItemService _quoteItemService;
		private readonly ICompanyDetailsRepository _companyDetailsRepository;

		public QuoteService(
			IUserContext applicationContext,
			IQuoteRepository quoteRepository,
			ICustomerRepository customerRepository,
			IEntityIdProvider entityIdProvider,
			ICurrencyRepository currencyRepository,
			QuoteItemService quoteItemService,
			ICompanyDetailsRepository companyDetailsRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_quoteRepository = quoteRepository;
			_customerRepository = customerRepository;
			_entityIdProvider = entityIdProvider;
			_quoteItemService = quoteItemService;
			_currencyRepository = currencyRepository;
			_companyDetailsRepository = companyDetailsRepository;
		}

		public Quote Create(Guid id, Guid customerId, string orderNumber, string adviceNumber, Guid currencyId)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the quote");
			var quote = new Quote();
			quote.Id = id;
			quote.QuoteNumber = _entityIdProvider.GetIdFor<Quote>();
			quote.Customer = GetCustomer(customerId);
			quote.CreatedBy = CurrentUser;
			quote.DateCreated = AppDateTime.GetUtcNow();
			quote.OrderNumber = orderNumber;
			quote.AdviceNumber = adviceNumber;
			quote.Currency = GetCurrency(currencyId);
			quote.IsActive = true;
			quote.Revision = 1;
			ValidateAnnotatedObjectThrowOnFailure(quote);
			_quoteRepository.Create(quote);
			return quote;
		}

		public void CreateQuotesFromPendingItems()
		{
			DoCreateQuotesFromPendingItems(_quoteItemService.GetPendingQuoteItems());
		}

		public void CreateQuotesFromPendingItems(IList<Guid> pendingItemIds)
		{
			DoCreateQuotesFromPendingItems(_quoteItemService.GetPendingQuoteItems(pendingItemIds));
		}

		public Quote Edit(Guid id, string orderNo, string adviceNo, Guid currencyId)
		{
			if (!CurrentUser.HasRole(UserRole.Admin | UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var quote = _quoteRepository.GetById(id);
			if (quote == null)
				throw new ArgumentException("A valid ID must be supplied for the quote item");
			quote.OrderNumber = orderNo;
			quote.AdviceNumber = adviceNo;
			quote.Currency = GetCurrency(currencyId);
			quote.Revision++;
			ValidateAnnotatedObjectThrowOnFailure(quote);
			_quoteRepository.Update(quote);
			return quote;
		}

		public Quote GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
			return _quoteRepository.GetById(id);
		}

		public int GetQuotesCount()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
			return _quoteRepository.GetQuotesCount();
		}

		public IEnumerable<Quote> GetQuotes()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
			return _quoteRepository.GetQuotes();
		}

		private void DoCreateQuotesFromPendingItems(IEnumerable<PendingQuoteItem> pendingItems)
		{
			var jobNoOrderNoGroups = pendingItems.GroupBy(g => new { g.JobItem.Job.JobNo, g.OrderNo });
			foreach (var group in jobNoOrderNoGroups)
			{
				var i = 0;
				var quoteId = Guid.NewGuid();
				foreach (var item in group.OrderBy(p => p.JobItem.ItemNo))
				{
					if (i++ == 0)
						Create(quoteId, item.Customer.Id, item.OrderNo, item.AdviceNo, _companyDetailsRepository.GetCompany().DefaultCurrency.Id);
					_quoteItemService.Create(Guid.NewGuid(), quoteId, item.JobItem.Id, item.Labour, item.Calibration, item.Parts, item.Carriage, item.Investigation, item.Report, item.Days, item.BeyondEconomicRepair);
					_quoteItemService.DeletePendingQuoteItem(item.Id);
				}
			}
		}

		private Customer GetCustomer(Guid customerId)
		{
			var customer = _customerRepository.GetById(customerId);
			if (customer == null)
				throw new ArgumentException("A valid ID must be supplied for the customer.");
			return customer;
		}

		private Currency GetCurrency(Guid currencyId)
		{
			var currency = _currencyRepository.GetById(currencyId);
			if (currency == null)
				throw new ArgumentException("A valid ID must be supplied for the currency");
			return currency;
		}
	}
}