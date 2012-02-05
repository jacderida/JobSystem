using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel.Entities;
using JobSystem.Resources.Suppliers;

namespace JobSystem.Mvc.ViewModels.Suppliers
{
	public class SupplierCreateViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Address Line 1")]
		public string Address1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Address Line 2")]
		public string Address2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Town/City")]
		public string Address3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "County")]
		public string Address4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Post Code")]
		public string Address5 { get; set; }

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
		public string Telephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Fax")]
		public string Fax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Email")]
		public string Email { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "First Contact Name")]
		public string Contact1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Second Contact Name")]
		public string Contact2 { get; set; }

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