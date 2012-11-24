using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.TaxCodes;

namespace JobSystem.DataModel.Entities
{
	public class TaxCode
	{
		public virtual Guid Id { get; set; }
		[Required(ErrorMessageResourceName = "TaxCodeEmpty", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(10, ErrorMessageResourceName = "TaxCodeName", ErrorMessageResourceType = typeof(Messages))]
		public virtual string TaxCodeName { get; set; }
		[Required(ErrorMessageResourceName = "DescriptionEmpty", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "DescriptionTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Description { get; set; }
		public virtual double Rate { get; set; }
	}
}