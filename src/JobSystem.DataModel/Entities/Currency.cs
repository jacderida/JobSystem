using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Currencies;

namespace JobSystem.DataModel.Entities
{
    public class Currency
    {
        public virtual Guid Id { get; set; }
        [Required(ErrorMessageResourceName = "NameNotSupplied", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "NameTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Name { get; set; }
        [Required(ErrorMessageResourceName = "DisplayMessageNotSupplied", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(50, ErrorMessageResourceName = "DisplayMessageTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string DisplayMessage { get; set; }
    }
}