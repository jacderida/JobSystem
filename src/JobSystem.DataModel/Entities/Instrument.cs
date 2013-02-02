using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
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
        public virtual int AllocatedCalibrationTime { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}, ", Manufacturer);
            sb.AppendFormat("{0}, ", ModelNo);
            if (!String.IsNullOrEmpty(Range) && Range.Trim() != "Not Specified")
                sb.AppendFormat("{0}, ", Range);
            if (!String.IsNullOrEmpty(Description) && Description.Trim() != "Not Specified")
                sb.AppendFormat("{0}, ", Description);
            return sb.ToString().Trim(", ".ToCharArray());
        }
    }
}