using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel.Dto;
using JobSystem.Resources.Suppliers;
using System;

namespace JobSystem.Mvc.ViewModels.Suppliers
{
    public class SupplierIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Address Line 1")]
        public string TradingAddress1 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Address Line 2")]
        public string TradingAddress2 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Town/City")]
        public string TradingAddress3 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "County")]
        public string TradingAddress4 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Post Code")]
        public string TradingAddress5 { get; set; }

        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Address Line 1")]
        public string SalesAddress1 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Address Line 2")]
        public string SalesAddress2 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Town/City")]
        public string SalesAddress3 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "County")]
        public string SalesAddress4 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Post Code")]
        public string SalesAddress5 { get; set; }

        [Display(Name = "Telephone")]
        public string TradingTelephone { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Fax")]
        public string TradingFax { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Email")]
        public string TradingEmail { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "First Contact Name")]
        public string TradingContact1 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Second Contact Name")]
        public string TradingContact2 { get; set; }

        [Display(Name = "Telephone")]
        public string SalesTelephone { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Fax")]
        public string SalesFax { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Email")]
        public string SalesEmail { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "First Contact Name")]
        public string SalesContact1 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Second Contact Name")]
        public string SalesContact2 { get; set; }
        
    }
}