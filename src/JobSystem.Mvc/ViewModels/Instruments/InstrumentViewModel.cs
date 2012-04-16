using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Instruments
{
    public class InstrumentViewModel
	{
		public Guid Id { get; set; }
		[Required]
		public string Manufacturer { get; set; }
		[Required]
		[Display(Name="Model Number")]
		public string ModelNo { get; set; }
		public string Range { get; set; }
		public string Description { get; set; }
		[Display(Name = "Allocated Calibration Time")]
		public int CalibrationTime { get; set; }
	}  
}
