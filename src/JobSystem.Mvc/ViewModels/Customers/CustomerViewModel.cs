using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel.Entities;
using JobSystem.Resources.Customers;

namespace JobSystem.Mvc.ViewModels.Customers
{
	public class CustomerViewModel
	{
		public Guid Id { get; set; }
		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Name { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name="Address Line 1")]
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
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Telephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Fax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Email { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "First Contact Name")]
		public string Contact1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Second Contact Name")]
		public string Contact2 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "InvoiceTitleTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Invoice Title")]
		public string InvoiceTitle { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Address Line 1")]
		public string InvoiceAddress1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Address Line 2")]
		public string InvoiceAddress2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Town/City")]
		public string InvoiceAddress3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "County")]
		public string InvoiceAddress4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
        [Display(Name = "Post Code")]
		public string InvoiceAddress5 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
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
		[StringLength(255, ErrorMessageResourceName = "DeliveryTitleTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Delivery Title")]
		public string DeliveryTitle { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Address Line 1")]
		public string DeliveryAddress1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Address Line 2")]
		public string DeliveryAddress2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Town/City")]
		public string DeliveryAddress3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "County")]
		public string DeliveryAddress4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Post Code")]
		public string DeliveryAddress5 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Telephone")]
		public string DeliveryTelephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Fax")]
		public string DeliveryFax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Email")]
		public string DeliveryEmail { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "First Contact Name")]
		public string DeliveryContact1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Display(Name = "Second Contact Name")]
		public string DeliveryContact2 { get; set; }
		[Display(Name = "Asset Line")]
		public string AssetLine { get; set; }
		public bool InvoiceAddressSameAsMain { get; set; }
		public bool DeliveryAddressSameAsMain { get; set; }

		public CustomerViewModel()
		{
			InvoiceAddressSameAsMain = true;
			DeliveryAddressSameAsMain = true;
		}

		public static CustomerViewModel GetCustomerViewModelFromCustomer(Customer customer)
		{
			return new CustomerViewModel
			{
				Id = customer.Id,
				Name = customer.Name,
				Address1 = !String.IsNullOrEmpty(customer.Address1) ? customer.Address1 : String.Empty,
				Address2 = !String.IsNullOrEmpty(customer.Address2) ? customer.Address2 : String.Empty,
				Address3 = !String.IsNullOrEmpty(customer.Address3) ? customer.Address3 : String.Empty,
				Address4 = !String.IsNullOrEmpty(customer.Address4) ? customer.Address4 : String.Empty,
				Address5 = !String.IsNullOrEmpty(customer.Address5) ? customer.Address5 : String.Empty,
				Telephone = !String.IsNullOrEmpty(customer.Telephone) ? customer.Telephone : String.Empty,
				Fax = !String.IsNullOrEmpty(customer.Fax) ? customer.Fax : String.Empty,
				Email = !String.IsNullOrEmpty(customer.Email) ? customer.Email : String.Empty,
				Contact1 = !String.IsNullOrEmpty(customer.Contact1) ? customer.Contact1 : String.Empty,
				Contact2 = !String.IsNullOrEmpty(customer.Contact2) ? customer.Contact2 : String.Empty,
				InvoiceTitle = !String.IsNullOrEmpty(customer.InvoiceTitle) ? customer.InvoiceTitle : String.Empty,
				InvoiceAddress1 = !String.IsNullOrEmpty(customer.InvoiceAddress1) ? customer.InvoiceAddress1 : String.Empty,
				InvoiceAddress2 = !String.IsNullOrEmpty(customer.InvoiceAddress2) ? customer.InvoiceAddress2 : String.Empty,
				InvoiceAddress3 = !String.IsNullOrEmpty(customer.InvoiceAddress3) ? customer.InvoiceAddress3 : String.Empty,
				InvoiceAddress4 = !String.IsNullOrEmpty(customer.InvoiceAddress4) ? customer.InvoiceAddress4 : String.Empty,
				InvoiceAddress5 = !String.IsNullOrEmpty(customer.InvoiceAddress5) ? customer.InvoiceAddress5 : String.Empty,
				SalesTelephone = !String.IsNullOrEmpty(customer.SalesTelephone) ? customer.SalesTelephone : String.Empty,
				SalesFax = !String.IsNullOrEmpty(customer.SalesFax) ? customer.SalesFax : String.Empty,
				SalesEmail = !String.IsNullOrEmpty(customer.SalesEmail) ? customer.SalesEmail : String.Empty,
				SalesContact1 = !String.IsNullOrEmpty(customer.SalesContact1) ? customer.SalesContact1 : String.Empty,
				SalesContact2 = !String.IsNullOrEmpty(customer.SalesContact2) ? customer.SalesContact2 : String.Empty,
				DeliveryTitle = !String.IsNullOrEmpty(customer.DeliveryTitle) ? customer.DeliveryTitle : String.Empty,
				DeliveryAddress1 = !String.IsNullOrEmpty(customer.DeliveryAddress1) ? customer.DeliveryAddress1 : String.Empty,
				DeliveryAddress2 = !String.IsNullOrEmpty(customer.DeliveryAddress2) ? customer.DeliveryAddress2 : String.Empty,
				DeliveryAddress3 = !String.IsNullOrEmpty(customer.DeliveryAddress3) ? customer.DeliveryAddress3 : String.Empty,
				DeliveryAddress4 = !String.IsNullOrEmpty(customer.DeliveryAddress4) ? customer.DeliveryAddress4 : String.Empty,
				DeliveryAddress5 = !String.IsNullOrEmpty(customer.DeliveryAddress5) ? customer.DeliveryAddress5 : String.Empty,
				DeliveryTelephone = !String.IsNullOrEmpty(customer.DeliveryTelephone) ? customer.DeliveryTelephone : String.Empty,
				DeliveryFax = !String.IsNullOrEmpty(customer.DeliveryFax) ? customer.DeliveryFax : String.Empty,
				DeliveryEmail = !String.IsNullOrEmpty(customer.DeliveryEmail) ? customer.DeliveryEmail : String.Empty,
				DeliveryContact1 = !String.IsNullOrEmpty(customer.DeliveryContact1) ? customer.DeliveryContact1 : String.Empty,
				DeliveryContact2 = !String.IsNullOrEmpty(customer.DeliveryContact2) ? customer.DeliveryContact2 : String.Empty,
				AssetLine = !String.IsNullOrEmpty(customer.AssetLine) ? customer.AssetLine : String.Empty,
			};
		}
	}
}