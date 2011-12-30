using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Instruments
{
    public class InstrumentViewModel
	{
		public string Id { get; set; }
		public string Manufacturer { get; set; }
		[Display(Name="Model Number")]
		public string ModelNo { get; set; }
		public string Range { get; set; }
		public string Description { get; set; }
	}  
}
