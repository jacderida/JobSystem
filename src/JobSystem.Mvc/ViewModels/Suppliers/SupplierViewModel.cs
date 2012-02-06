using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel.Entities;
using JobSystem.Resources.Suppliers;

namespace JobSystem.Mvc.ViewModels.Suppliers
{
	public class SupplierViewModel
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

		public static SupplierViewModel GetSupplierViewModelFromSupplier(Supplier supplier)
		{
			return new SupplierViewModel
			{
				Id = supplier.Id,
				Name = supplier.Name,
				Address1 = !String.IsNullOrEmpty(supplier.Address1) ? supplier.Address1 : String.Empty,
				Address2 = !String.IsNullOrEmpty(supplier.Address2) ? supplier.Address2 : String.Empty,
				Address3 = !String.IsNullOrEmpty(supplier.Address3) ? supplier.Address3 : String.Empty,
				Address4 = !String.IsNullOrEmpty(supplier.Address4) ? supplier.Address4 : String.Empty,
				Address5 = !String.IsNullOrEmpty(supplier.Address5) ? supplier.Address5 : String.Empty,
				Telephone = !String.IsNullOrEmpty(supplier.Telephone) ? supplier.Telephone : String.Empty,
				Fax = !String.IsNullOrEmpty(supplier.Fax) ? supplier.Fax : String.Empty,
				Email = !String.IsNullOrEmpty(supplier.Email) ? supplier.Email : String.Empty,
				Contact1 = !String.IsNullOrEmpty(supplier.Contact1) ? supplier.Contact1 : String.Empty,
				Contact2 = !String.IsNullOrEmpty(supplier.Contact2) ? supplier.Contact2 : String.Empty,
				SalesAddress1 = !String.IsNullOrEmpty(supplier.SalesAddress1) ? supplier.SalesAddress1 : String.Empty,
				SalesAddress2 = !String.IsNullOrEmpty(supplier.SalesAddress2) ? supplier.SalesAddress2 : String.Empty,
				SalesAddress3 = !String.IsNullOrEmpty(supplier.SalesAddress3) ? supplier.SalesAddress3 : String.Empty,
				SalesAddress4 = !String.IsNullOrEmpty(supplier.SalesAddress4) ? supplier.SalesAddress4 : String.Empty,
				SalesAddress5 = !String.IsNullOrEmpty(supplier.SalesAddress5) ? supplier.SalesAddress5 : String.Empty,
				SalesTelephone = !String.IsNullOrEmpty(supplier.SalesTelephone) ? supplier.SalesTelephone : String.Empty,
				SalesFax = !String.IsNullOrEmpty(supplier.SalesFax) ? supplier.SalesFax : String.Empty,
				SalesEmail = !String.IsNullOrEmpty(supplier.SalesEmail) ? supplier.SalesEmail : String.Empty,
				SalesContact1 = !String.IsNullOrEmpty(supplier.SalesContact1) ? supplier.SalesContact1 : String.Empty,
				SalesContact2 = !String.IsNullOrEmpty(supplier.SalesContact2) ? supplier.SalesContact2 : String.Empty
			};
		}
	}
}