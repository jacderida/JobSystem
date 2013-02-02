using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.TestStandards;

namespace JobSystem.DataModel.Entities
{
    [Serializable]
    public class TestStandard
    {
        public virtual Guid Id { get; set; }
        [Required(ErrorMessageResourceName = "DescriptionNotSupplied", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(255, ErrorMessageResourceName = "DescriptionTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Description { get; set; }
        [Required(ErrorMessageResourceName = "SerialNoNotSupplied", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "SerialNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string SerialNo { get; set; }
        [Required(ErrorMessageResourceName = "CertNoNotSupplied", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "CertNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string CertificateNo { get; set; }
    }
}