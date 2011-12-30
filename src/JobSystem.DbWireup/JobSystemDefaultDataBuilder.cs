using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;

namespace JobSystem.DbWireup
{
	public class JobSystemDefaultDataBuilder
	{
		private List<ListItem> _jobTypes;
		private List<ListItem> _jobItemLocations;
		private List<ListItem> _jobItemWorkStatusItems;
		private List<ListItem> _jobItemInitialStatusItems;
		private List<ListItem> _jobItemStatusItems;
		private List<ListItem> _jobItemWorkTypes;
		private List<ListItem> _jobItemFields;

		private List<ListItem> _paymentTerms;
		private List<TaxCode> _taxCodes;
		private List<ListItem> _currencies;
		private List<BankDetails> _bankDetails;
		private List<EntityIdLookup> _entityIdLookups;

		public JobSystemDefaultDataBuilder()
		{
			_jobTypes = new List<ListItem>();
			_jobItemLocations = new List<ListItem>();
			_jobItemWorkStatusItems = new List<ListItem>();
			_jobItemInitialStatusItems = new List<ListItem>();
			_jobItemStatusItems = new List<ListItem>();
			_jobItemWorkTypes = new List<ListItem>();
			_jobItemFields = new List<ListItem>();
			_paymentTerms = new List<ListItem>();
			_taxCodes = new List<TaxCode>();
			_currencies = new List<ListItem>();
			_bankDetails = new List<BankDetails>();
			_entityIdLookups = new List<EntityIdLookup>();
		}

		public JobSystemDefaultData Build()
		{
			var defaultData = new JobSystemDefaultData();
			foreach (var type in _jobTypes)
				defaultData.JobTypes.Add(type);
			foreach (var location in _jobItemLocations)
				defaultData.JobItemLocations.Add(location);
			foreach (var statusItem in _jobItemWorkStatusItems)
				defaultData.JobItemWorkStatusItems.Add(statusItem);
			foreach (var statusItem in _jobItemInitialStatusItems)
				defaultData.JobItemInitialStatusItems.Add(statusItem);
			foreach (var statusItem in _jobItemStatusItems)
				defaultData.JobItemStatusItems.Add(statusItem);
			foreach (var workType in _jobItemWorkTypes)
				defaultData.JobItemWorkTypes.Add(workType);
			foreach (var field in _jobItemFields)
				defaultData.JobItemFields.Add(field);
			foreach (var paymentTerm in _paymentTerms)
				defaultData.PaymentTerms.Add(paymentTerm);
			foreach (var taxCode in _taxCodes)
				defaultData.TaxCodes.Add(taxCode);
			foreach (var currency in _currencies)
				defaultData.Currencies.Add(currency);
			foreach (var bankDetails in _bankDetails)
				defaultData.BankDetails.Add(bankDetails);
			foreach (var entityLookup in _entityIdLookups)
				defaultData.EntityIdLookups.Add(entityLookup);
			return defaultData;
		}

		public JobSystemDefaultDataBuilder WithJobTypes(params string[] jobTypes)
		{
			foreach (var type in jobTypes)
				_jobTypes.Add(new ListItem { Id = Guid.NewGuid(), Name = type, Type = ListItemType.JobType });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemLocations(params string[] workLocations)
		{
			foreach (var location in workLocations)
				_jobItemLocations.Add(new ListItem { Id = Guid.NewGuid(), Name = location, Type = ListItemType.JobItemLocation });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemWorkStatusItems(params string[] workStatusItems)
		{
			foreach (var statusItem in workStatusItems)
				_jobItemWorkStatusItems.Add(new ListItem { Id = Guid.NewGuid(), Name = statusItem, Type = ListItemType.JobItemWorkStatus });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemInitialStatusItems(params string[] initialStatusItems)
		{
			foreach (var statusItem in initialStatusItems)
				_jobItemInitialStatusItems.Add(new ListItem { Id = Guid.NewGuid(), Name = statusItem, Type = ListItemType.JobItemInitialStatus });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemStatusItems(params string[] statusItems)
		{
			foreach (var statusItem in statusItems)
				_jobItemStatusItems.Add(new ListItem { Id = Guid.NewGuid(), Name = statusItem, Type = ListItemType.JobItemStatus });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemWorkTypes(params string[] workTypes)
		{
			foreach (var workType in workTypes)
				_jobItemWorkTypes.Add(new ListItem { Id = Guid.NewGuid(), Name = workType, Type = ListItemType.JobItemWorkType });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemFields(params string[] fields)
		{
			foreach (var workType in fields)
				_jobItemFields.Add(new ListItem { Id = Guid.NewGuid(), Name = workType, Type = ListItemType.JobItemField });
			return this;
		}

		public JobSystemDefaultDataBuilder WithPaymentTerms(params Tuple<Guid, string>[] paymentTerms)
		{
			foreach (var paymentTerm in paymentTerms)
				_paymentTerms.Add(new ListItem { Id = paymentTerm.Item1, Name = paymentTerm.Item2, Type = ListItemType.PaymentTerm });
			return this;
		}

		public JobSystemDefaultDataBuilder WithTaxCodes(params TaxCode[] taxCodes)
		{
			foreach (var taxCode in taxCodes)
				_taxCodes.Add(new TaxCode { Id = taxCode.Id, TaxCodeName = taxCode.TaxCodeName, Description = taxCode.Description, Rate = taxCode.Rate });
			return this;
		}

		public JobSystemDefaultDataBuilder WithCurrencies(params Tuple<Guid, string>[] currencies)
		{
			foreach (var currency in currencies)
				_currencies.Add(new ListItem { Id = currency.Item1, Name = currency.Item2, Type = ListItemType.Currency });
			return this;
		}

		public JobSystemDefaultDataBuilder WithBankDetails(params BankDetails[] bankDetailsList)
		{
			foreach (var bankDetails in bankDetailsList)
				_bankDetails.Add(bankDetails);
			return this;
		}

		public JobSystemDefaultDataBuilder WithEntitySeeds(params Tuple<Type, int, string>[] seeds)
		{
			foreach (var seed in seeds)
			{
				_entityIdLookups.Add(
					new EntityIdLookup
					{
						Id = Guid.NewGuid(),
						EntityTypeName = seed.Item1.FullName,
						NextId = seed.Item2,
						Prefix = seed.Item3
					});
			}
			return this;
		}
	}
}