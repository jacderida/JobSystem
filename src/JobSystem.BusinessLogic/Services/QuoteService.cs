using System;
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
		private readonly ICustomerRepository _customerRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly IListItemRepository _listItemRepository;
		private readonly QuoteItemService _quoteItemService;

		public QuoteService(
			IUserContext applicationContext,
			IQuoteRepository quoteRepository,
			ICustomerRepository customerRepository,
			IEntityIdProvider entityIdProvider,
			IListItemRepository listItemRepository,
			QuoteItemService quoteItemService,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_quoteRepository = quoteRepository;
			_customerRepository = customerRepository;
			_entityIdProvider = entityIdProvider;
			_quoteItemService = quoteItemService;
			_listItemRepository = listItemRepository;
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
			ValidateAnnotatedObjectThrowOnFailure(quote);
			_quoteRepository.Create(quote);
			return quote;
		}

		private Customer GetCustomer(Guid customerId)
		{
			var customer = _customerRepository.GetById(customerId);
			if (customer == null)
				throw new ArgumentException("A valid ID must be supplied for the customer.");
			return customer;
		}

		private ListItem GetCurrency(Guid currencyId)
		{
			var currency = _listItemRepository.GetById(currencyId);
			if (currency == null)
				throw new ArgumentException("A valid ID must be supplied for the currency");
			if (currency.Category.Type != ListItemCategoryType.Currency)
				throw new ArgumentException("The list item must be of type currency");
			return currency;
		}
	}
}