using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace JobSystem.Mvc.ViewModels.BankDetails
{
    public class BankDetailsIndexViewModel
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string AccountNo { get; set; }
        public string SortCode { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Iban { get; set; }
    }
}