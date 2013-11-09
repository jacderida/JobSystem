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
        [Required(ErrorMessageResourceName = "TelephoneRequired", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "TelephoneTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Telephone { get; set; }
        [Required(ErrorMessageResourceName = "FaxRequired", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "FaxTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Fax { get; set; }
        [StringLength(50, ErrorMessageResourceName = "RegNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string RegNo { get; set; }
        [StringLength(50, ErrorMessageResourceName = "VatRegNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string VatRegNo { get; set; }
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "EmailTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Email { get; set; }
        [Required(ErrorMessageResourceName = "WwwRequired", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(255, ErrorMessageResourceName = "WwwTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Www{ get; set; }
        [Required(ErrorMessageResourceName = "TermsAndConditionsRequired", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(2000, ErrorMessageResourceName = "TermsAndConditionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string TermsAndConditions { get; set; }
		public virtual string QuoteSummaryText { get; set; }
		public virtual string OrderAcknowledgeText { get; set; }
        public virtual Currency DefaultCurrency { get; set; }
        public virtual TaxCode DefaultTaxCode { get; set; }
        public virtual ListItem DefaultPaymentTerm { get; set; }
        public virtual BankDetails DefaultBankDetails { get; set; }
        public virtual byte[] MainLogo { get; set; }
        public virtual bool ApplyAllPrices { get; set; }
        public virtual string DefaultCultureCode { get; set; }
    }
}