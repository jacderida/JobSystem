using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Customers;

namespace JobSystem.DataModel.Entities
{
	public class Customer
	{
		public virtual Guid Id { get; set; }
		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Name { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AssetLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string AssetLine { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address5 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Telephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Fax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Email { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Contact1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Contact2 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "InvoiceTitleTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string InvoiceTitle { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string InvoiceAddress1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string InvoiceAddress2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string InvoiceAddress3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string InvoiceAddress4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string InvoiceAddress5 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesTelephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesFax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesEmail { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesContact1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesContact2 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "DeliveryTitleTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryTitle { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress5 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress6 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryAddress7 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryTelephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryFax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryEmail { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryContact1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactInfoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string DeliveryContact2 { get; set; }
	}
}