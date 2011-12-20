using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.CompanyDetails;

namespace JobSystem.Mvc.ViewModels.Admin
{
	public class CompanyDetailsViewModel
	{
		public Guid Id { get; set; }

		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Name { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Address1 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Address2 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Address3 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Address4 { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AddressLineTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Address5 { get; set; }
		[Required(ErrorMessageResourceName = "TelephoneRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "TelephoneTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Telephone { get; set; }
		[Required(ErrorMessageResourceName = "FaxRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "FaxTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Fax { get; set; }
		[StringLength(50, ErrorMessageResourceName = "RegNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string RegNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "VatRegNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string VatRegNo { get; set; }
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "EmailTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Email { get; set; }
		[Required(ErrorMessageResourceName = "WwwRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, ErrorMessageResourceName = "WwwTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Www { get; set; }
		[Required(ErrorMessageResourceName = "TermsAndConditionsRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(2000, ErrorMessageResourceName = "TermsAndConditionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string TermsAndConditions { get; set; }

		public Guid CurrencyId { get; set; }
		public Guid TaxCodeId { get; set; }
		public Guid PaymentTermId { get; set; }
		public Guid BankDetailsId { get; set; }
		public IEnumerable<SelectListItem> Currencies { get; set; }
		public IEnumerable<SelectListItem> TaxCodes { get; set; }
		public IEnumerable<SelectListItem> PaymentTerms { get; set; }
		public IEnumerable<SelectListItem> BankDetails { get; set; }
	}
}