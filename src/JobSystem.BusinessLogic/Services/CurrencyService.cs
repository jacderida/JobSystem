using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Currencies;

namespace JobSystem.BusinessLogic.Services
{
	public class CurrencyService : ServiceBase
	{
		private readonly ICurrencyRepository _currencyRepository;

		public CurrencyService(
			IUserContext applicationContext,
			ICurrencyRepository currencyRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_currencyRepository = currencyRepository;
		}

		public Currency Create(Guid id, string name, string displayMessage)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the currency");
			var currency = new Currency();
			currency.Id = id;
			currency.Name = name;
			currency.DisplayMessage = displayMessage;
			ValidateAnnotatedObjectThrowOnFailure(currency);
			_currencyRepository.Create(currency);
			return currency;
		}

		public Currency Edit(Guid id, string name, string displayMessage)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			var currency = _currencyRepository.GetById(id);
			if (currency == null)
				throw new ArgumentException("A valid ID must be supplied for the currency");
			if (IsSystemCurrency(currency))
				throw new DomainValidationException(Messages.EditProhibited);
			currency.Name = name;
			currency.DisplayMessage = displayMessage;
			ValidateAnnotatedObjectThrowOnFailure(currency);
			_currencyRepository.Update(currency);
			return currency;
		}

		public Currency GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _currencyRepository.GetById(id);
		}

		public IEnumerable<Currency> GetCurrencies()
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _currencyRepository.GetCurrencies();
		}

		private bool IsSystemCurrency(Currency currency)
		{
			return currency.Name == "GBP" || currency.Name == "EUR" || currency.Name == "USD";
		}
	}
}