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
		private List<ListItemCategory> _listItemCategories;
		private List<ListItem> _jobTypes;
		private List<ListItem> _jobItemLocations;
		private List<ListItem> _jobItemInitialLocations;
		private List<ListItem> _jobItemWorkStatusItems;
		private List<ListItem> _jobItemInitialStatusItems;
		private List<ListItem> _jobItemStatusItems;
		private List<ListItem> _jobItemWorkTypes;
		private List<ListItem> _jobItemCategories;

		private List<ListItem> _paymentTerms;
		private List<TaxCode> _taxCodes;
		private List<ListItem> _currencies;
		private List<BankDetails> _bankDetails;
		private List<EntityIdLookup> _entityIdLookups;

		public JobSystemDefaultDataBuilder()
		{
			_listItemCategories = new List<ListItemCategory>();
			_jobTypes = new List<ListItem>();
			_jobItemLocations = new List<ListItem>();
			_jobItemInitialLocations = new List<ListItem>();
			_jobItemWorkStatusItems = new List<ListItem>();
			_jobItemInitialStatusItems = new List<ListItem>();
			_jobItemStatusItems = new List<ListItem>();
			_jobItemWorkTypes = new List<ListItem>();
			_jobItemCategories = new List<ListItem>();
			_paymentTerms = new List<ListItem>();
			_taxCodes = new List<TaxCode>();
			_currencies = new List<ListItem>();
			_bankDetails = new List<BankDetails>();
			_entityIdLookups = new List<EntityIdLookup>();
		}

		public JobSystemDefaultData Build()
		{
			var defaultData = new JobSystemDefaultData();
			_listItemCategories.ForEach(c => defaultData.ListItemCategories.Add(c));
			_jobTypes.ForEach(jt => defaultData.JobTypes.Add(jt));
			_jobItemLocations.ForEach(jil => defaultData.JobItemLocations.Add(jil));
			_jobItemInitialLocations.ForEach(i => defaultData.JobItemInitialLocations.Add(i));
			_jobItemWorkStatusItems.ForEach(si => defaultData.JobItemWorkStatusItems.Add(si));
			_jobItemInitialStatusItems.ForEach(si => defaultData.JobItemInitialStatusItems.Add(si));
			_jobItemStatusItems.ForEach(si => defaultData.JobItemStatusItems.Add(si));
			_jobItemWorkTypes.ForEach(wt => defaultData.JobItemWorkTypes.Add(wt));
			_jobItemCategories.ForEach(c => defaultData.JobItemCategories.Add(c));
			_paymentTerms.ForEach(pt => defaultData.PaymentTerms.Add(pt));
			_taxCodes.ForEach(tc => defaultData.TaxCodes.Add(tc));
			_currencies.ForEach(c => defaultData.Currencies.Add(c));
			_bankDetails.ForEach(bd => defaultData.BankDetails.Add(bd));
			_entityIdLookups.ForEach(e => defaultData.EntityIdLookups.Add(e));
			return defaultData;
		}

		public JobSystemDefaultDataBuilder WithListItemCategories(params Tuple<string, ListItemCategoryType>[] categories)
		{
			foreach (var category in categories)
				_listItemCategories.Add(new ListItemCategory { Id = Guid.NewGuid(), Name = category.Item1, Type = category.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobTypes(params Tuple<string, ListItemType>[] jobTypes)
		{
			foreach (var type in jobTypes)
				_jobTypes.Add(new ListItem { Id = Guid.NewGuid(), Name = type.Item1, Type = type.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemInitialLocations(params Tuple<string, ListItemType>[] workLocations)
		{
			foreach (var location in workLocations)
				_jobItemInitialLocations.Add(new ListItem { Id = Guid.NewGuid(), Name = location.Item1, Type = location.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemLocations(params Tuple<string, ListItemType>[] workLocations)
		{
			foreach (var location in workLocations)
				_jobItemLocations.Add(new ListItem { Id = Guid.NewGuid(), Name = location.Item1, Type = location.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemWorkStatusItems(params Tuple<string, ListItemType>[] workStatusItems)
		{
			foreach (var statusItem in workStatusItems)
				_jobItemWorkStatusItems.Add(new ListItem { Id = Guid.NewGuid(), Name = statusItem.Item1, Type = statusItem.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemInitialStatusItems(params Tuple<string, ListItemType>[] initialStatusItems)
		{
			foreach (var statusItem in initialStatusItems)
				_jobItemInitialStatusItems.Add(new ListItem { Id = Guid.NewGuid(), Name = statusItem.Item1, Type = statusItem.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemStatusItems(params Tuple<string, ListItemType>[] statusItems)
		{
			foreach (var statusItem in statusItems)
				_jobItemStatusItems.Add(new ListItem { Id = Guid.NewGuid(), Name = statusItem.Item1, Type = statusItem.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemWorkTypes(params Tuple<string, ListItemType>[] workTypes)
		{
			foreach (var workType in workTypes)
				_jobItemWorkTypes.Add(new ListItem { Id = Guid.NewGuid(), Name = workType.Item1, Type = workType.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemCategories(params Tuple<string, ListItemType>[] categories)
		{
			foreach (var category in categories)
				_jobItemCategories.Add(new ListItem { Id = Guid.NewGuid(), Name = category.Item1, Type = category.Item2 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithPaymentTerms(params Tuple<Guid, string, ListItemType>[] paymentTerms)
		{
			foreach (var paymentTerm in paymentTerms)
				_paymentTerms.Add(new ListItem { Id = paymentTerm.Item1, Name = paymentTerm.Item2, Type = paymentTerm.Item3 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithTaxCodes(params TaxCode[] taxCodes)
		{
			foreach (var taxCode in taxCodes)
				_taxCodes.Add(new TaxCode { Id = taxCode.Id, TaxCodeName = taxCode.TaxCodeName, Description = taxCode.Description, Rate = taxCode.Rate });
			return this;
		}

		public JobSystemDefaultDataBuilder WithCurrencies(params Tuple<Guid, string, ListItemType>[] currencies)
		{
			foreach (var currency in currencies)
				_currencies.Add(new ListItem { Id = currency.Item1, Name = currency.Item2, Type = currency.Item3 });
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