using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.CompanyDetails;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class CompanyDetails
	{
		public virtual Guid Id { get; set; }
		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Name { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address1 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address2 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address3 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address4 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Address5 { get; set; }
		[StringLength(50, ErrorMessageResourceName = "TelephoneTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Telephone { get; set; }
		[StringLength(50, ErrorMessageResourceName = "FaxTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Fax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "RegNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string RegNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "VatRegNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string VatRegNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "EmailTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Email { get; set; }
		[StringLength(50, ErrorMessageResourceName = "WwwlTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Www{ get; set; }
		[StringLength(2000, ErrorMessageResourceName = "TermsAndConditionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string TermsAndConditions { get; set; }
		public virtual Currency DefaultCurrency { get; set; }
		public virtual TaxCode DefaultTaxCode { get; set; }
		public virtual PaymentTerm DefaultPaymentTerm { get; set; }
		public virtual BankDetails DefaultBankDetails { get; set; }
		public virtual byte[] MainLogo { get; set; }
	}
}