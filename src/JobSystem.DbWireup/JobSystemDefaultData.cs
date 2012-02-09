using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DbWireup
{
	public class JobSystemDefaultData
	{
		public List<ListItemCategory> ListItemCategories { get; private set; }
		public List<ListItem> JobTypes { get; private set; }
		public List<ListItem> JobItemWorkStatusItems { get; private set; }
		public List<ListItem> JobItemInitialStatusItems { get; private set; }
		public List<ListItem> JobItemStatusItems { get; private set; }
		public List<ListItem> JobItemWorkTypes { get; private set; }
		public List<ListItem> JobItemCategories { get; private set; }
		public List<ListItem> JobItemLocations { get; private set; }
		public List<ListItem> JobItemInitialLocations { get; private set; }

		public List<ListItem> PaymentTerms { get; set; }
		public List<TaxCode> TaxCodes { get; set; }
		public List<ListItem> Currencies { get; set; }
		public List<BankDetails> BankDetails { get; set; }
		public List<EntityIdLookup> EntityIdLookups { get; set; }

		public JobSystemDefaultData()
		{
			ListItemCategories = new List<ListItemCategory>();
			JobTypes = new List<ListItem>();
			JobItemWorkStatusItems = new List<ListItem>();
			JobItemInitialStatusItems = new List<ListItem>();
			JobItemStatusItems = new List<ListItem>();
			JobItemWorkTypes = new List<ListItem>();
			JobItemCategories = new List<ListItem>();
			JobItemLocations = new List<ListItem>();
			JobItemInitialLocations = new List<ListItem>();
			PaymentTerms = new List<ListItem>();
			TaxCodes = new List<TaxCode>();
			Currencies = new List<ListItem>();
			BankDetails = new List<BankDetails>();
			EntityIdLookups = new List<EntityIdLookup>();
		}
	}
}