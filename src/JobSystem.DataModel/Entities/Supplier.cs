using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Suppliers;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Supplier
	{
		public virtual Guid Id { get; set; }
		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Name { get; set; }
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
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesAddress1 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesAddress2 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesAddress3 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesAddress4 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SalesAddress5 { get; set; }
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
	}
}