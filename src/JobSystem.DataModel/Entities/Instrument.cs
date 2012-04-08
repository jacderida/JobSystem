using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Instruments;
using System.Text;

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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}, {1},", Manufacturer, ModelNo);
			if (Range != "Not Specified")
				sb.AppendFormat("{0}, ", Range);
			sb.Append(Description);
			return sb.ToString();
		}
	}
}