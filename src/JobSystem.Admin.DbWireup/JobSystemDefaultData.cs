using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.Admin.DbWireup
{
    public class JobSystemDefaultData
    {
        public List<ListItemCategory> ListItemCategories { get; private set; }
        public List<Tuple<Guid, ListItem>> JobTypes { get; private set; }
        public List<Tuple<Guid, ListItem>> CertificateTypes { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemWorkStatusItems { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemInitialStatusItems { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemStatusItems { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemWorkTypes { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemCategories { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemLocations { get; private set; }
        public List<Tuple<Guid, ListItem>> JobItemInitialLocations { get; private set; }
        public List<Tuple<Guid, ListItem>> PaymentTerms { get; set; }
        public List<TaxCode> TaxCodes { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<BankDetails> BankDetails { get; set; }
        public List<EntityIdLookup> EntityIdLookups { get; set; }

        public JobSystemDefaultData()
        {
            ListItemCategories = new List<ListItemCategory>();
            JobTypes = new List<Tuple<Guid, ListItem>>();
            CertificateTypes = new List<Tuple<Guid, ListItem>>();
            JobItemWorkStatusItems = new List<Tuple<Guid, ListItem>>();
            JobItemInitialStatusItems = new List<Tuple<Guid, ListItem>>();
            JobItemStatusItems = new List<Tuple<Guid, ListItem>>();
            JobItemWorkTypes = new List<Tuple<Guid, ListItem>>();
            JobItemCategories = new List<Tuple<Guid, ListItem>>();
            JobItemLocations = new List<Tuple<Guid, ListItem>>();
            JobItemInitialLocations = new List<Tuple<Guid, ListItem>>();
            PaymentTerms = new List<Tuple<Guid, ListItem>>();
            TaxCodes = new List<TaxCode>();
            Currencies = new List<Currency>();
            BankDetails = new List<BankDetails>();
            EntityIdLookups = new List<EntityIdLookup>();
        }
    }
}