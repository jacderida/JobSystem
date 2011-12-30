using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Instruments;

namespace JobSystem.DataModel.Entities
{
	public class Instrument
	{
		public virtual Guid Id { get; set; }
		[Required(ErrorMessageResourceName = "ManufacturerRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "ManufacturerTooLong", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Manufacturer { get; set; }
		[Required(ErrorMessageResourceName = "ModelNoRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "ModelNoTooLong", ErrorMessageResourceType = typeof(Messages))]
		public virtual string ModelNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "RangeTooLong", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Range { get; set; }
		[StringLength(50, ErrorMessageResourceName = "DescriptionTooLong", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Description { get; set; }
	}
}