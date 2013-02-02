using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace JobSystem.Mvc.ViewModels.BankDetails
{
    public class BankDetailsCreateViewModel
    {
        public Guid Id { get; set; }
        [Display(Name="Short Name")]
        public string ShortName { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNo { get; set; }
        [Display(Name = "Sort Code")]
        public string SortCode { get; set; }
        public string Name { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = "Address 3")]
        public string Address3 { get; set; }
        [Display(Name = "Address 4")]
        public string Address4 { get; set; }
        [Display(Name = "Address 5")]
        public string Address5 { get; set; }
        [Display(Name = "IBAN")]
        public string Iban { get; set; }
    }
}