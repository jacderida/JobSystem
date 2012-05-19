using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DbWireup
{
	public class JobSystemDefaultDataBuilder
	{
		private List<ListItemCategory> _listItemCategories;
		private List<Tuple<Guid, ListItem>> _jobTypes;
		private List<Tuple<Guid, ListItem>> _cerificateTypes;
		private List<Tuple<Guid, ListItem>> _jobItemWorkStatusItems;
		private List<Tuple<Guid, ListItem>> _jobItemInitialStatusItems;
		private List<Tuple<Guid, ListItem>> _jobItemStatusItems;
		private List<Tuple<Guid, ListItem>> _jobItemWorkTypes;
		private List<Tuple<Guid, ListItem>> _jobItemCategories;

		private List<Tuple<Guid, ListItem>> _paymentTerms;
		private List<TaxCode> _taxCodes;
		private List<Currency> _currencies;
		private List<BankDetails> _bankDetails;
		private List<EntityIdLookup> _entityIdLookups;

		public JobSystemDefaultDataBuilder()
		{
			_listItemCategories = new List<ListItemCategory>();
			_jobTypes = new List<Tuple<Guid, ListItem>>();
			_cerificateTypes = new List<Tuple<Guid, ListItem>>();
			_jobItemWorkStatusItems = new List<Tuple<Guid, ListItem>>();
			_jobItemInitialStatusItems = new List<Tuple<Guid, ListItem>>();
			_jobItemStatusItems = new List<Tuple<Guid, ListItem>>();
			_jobItemWorkTypes = new List<Tuple<Guid, ListItem>>();
			_jobItemCategories = new List<Tuple<Guid, ListItem>>();
			_paymentTerms = new List<Tuple<Guid, ListItem>>();
			_taxCodes = new List<TaxCode>();
			_currencies = new List<Currency>();
			_bankDetails = new List<BankDetails>();
			_entityIdLookups = new List<EntityIdLookup>();
		}

		public JobSystemDefaultData Build()
		{
			var defaultData = new JobSystemDefaultData();
			_listItemCategories.ForEach(c => defaultData.ListItemCategories.Add(c));
			_jobTypes.ForEach(jt => defaultData.JobTypes.Add(jt));
			_cerificateTypes.ForEach(ct => defaultData.CertificateTypes.Add(ct));
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

		public JobSystemDefaultDataBuilder WithListItemCategories(params Tuple<Guid, string, ListItemCategoryType>[] categories)
		{
			foreach (var category in categories)
				_listItemCategories.Add(new ListItemCategory { Id = category.Item1, Name = category.Item2, Type = category.Item3 });
			return this;
		}

		public JobSystemDefaultDataBuilder WithCertificateTypes(params Tuple<string, ListItemType, Guid>[] certificateTypes)
		{
			foreach (var type in certificateTypes)
				_cerificateTypes.Add(
					Tuple.Create<Guid, ListItem>(type.Item3, new ListItem { Id = Guid.NewGuid(), Name = type.Item1, Type = type.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobTypes(params Tuple<string, ListItemType, Guid>[] jobTypes)
		{
			foreach (var type in jobTypes)
				_jobTypes.Add(
					Tuple.Create<Guid, ListItem>(type.Item3,  new ListItem { Id = Guid.NewGuid(), Name = type.Item1, Type = type.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemWorkStatusItems(params Tuple<string, ListItemType, Guid>[] workStatusItems)
		{
			foreach (var statusItem in workStatusItems)
				_jobItemWorkStatusItems.Add(
					Tuple.Create<Guid, ListItem>(statusItem.Item3, new ListItem { Id = Guid.NewGuid(), Name = statusItem.Item1, Type = statusItem.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemInitialStatusItems(params Tuple<string, ListItemType, Guid>[] initialStatusItems)
		{
			foreach (var statusItem in initialStatusItems)
				_jobItemInitialStatusItems.Add(
					Tuple.Create<Guid, ListItem>(statusItem.Item3, new ListItem { Id = Guid.NewGuid(), Name = statusItem.Item1, Type = statusItem.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemStatusItems(params Tuple<string, ListItemType, Guid>[] statusItems)
		{
			foreach (var statusItem in statusItems)
				_jobItemStatusItems.Add(
					Tuple.Create<Guid, ListItem>(statusItem.Item3, new ListItem { Id = Guid.NewGuid(), Name = statusItem.Item1, Type = statusItem.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemWorkTypes(params Tuple<string, ListItemType, Guid>[] workTypes)
		{
			foreach (var workType in workTypes)
				_jobItemWorkTypes.Add(
					Tuple.Create<Guid, ListItem>(workType.Item3, new ListItem { Id = Guid.NewGuid(), Name = workType.Item1, Type = workType.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithJobItemCategories(params Tuple<string, ListItemType, Guid>[] categories)
		{
			foreach (var category in categories)
				_jobItemCategories.Add(
					Tuple.Create<Guid, ListItem>(category.Item3, new ListItem { Id = Guid.NewGuid(), Name = category.Item1, Type = category.Item2 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithPaymentTerms(params Tuple<Guid, string, ListItemType, Guid>[] paymentTerms)
		{
			foreach (var paymentTerm in paymentTerms)
				_paymentTerms.Add(
					Tuple.Create<Guid, ListItem>(paymentTerm.Item4, new ListItem { Id = paymentTerm.Item1, Name = paymentTerm.Item2, Type = paymentTerm.Item3 }));
			return this;
		}

		public JobSystemDefaultDataBuilder WithTaxCodes(params TaxCode[] taxCodes)
		{
			foreach (var taxCode in taxCodes)
				_taxCodes.Add(new TaxCode { Id = taxCode.Id, TaxCodeName = taxCode.TaxCodeName, Description = taxCode.Description, Rate = taxCode.Rate });
			return this;
		}

		public JobSystemDefaultDataBuilder WithCurrencies(params Currency[] currencies)
		{
			foreach (var currency in currencies)
				_currencies.Add(new Currency { Id = currency.Id, Name = currency.Name, DisplayMessage = currency.DisplayMessage });
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