using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.CompanyDetails;

namespace JobSystem.DataModel.Entities
{
    public class BankDetails
    {
        public virtual Guid Id { get; set; }
        [Required(ErrorMessageResourceName = "ShortNameRequired", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        [StringLength(50, ErrorMessageResourceName = "ShortNameTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string ShortName { get; set; }
        [Required(ErrorMessageResourceName = "AccountNoRequired", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        [StringLength(50, ErrorMessageResourceName = "AccountNoTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string AccountNo { get; set; }
        [Required(ErrorMessageResourceName = "SortCodeRequired", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        [StringLength(50, ErrorMessageResourceName = "SortCodeTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string SortCode { get; set; }
        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        [StringLength(255, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Name { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Address1 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Address2 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Address3 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Address4 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "AddressTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Address5 { get; set; }
        [StringLength(50, ErrorMessageResourceName = "IbanTooLarge", ErrorMessageResourceType = typeof(BankDetailsMessages))]
        public virtual string Iban { get; set; }
    }
}