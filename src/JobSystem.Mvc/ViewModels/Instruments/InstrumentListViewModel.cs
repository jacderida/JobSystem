using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Instruments
{
	public class InstrumentListViewModel : PageViewModel
	{
		public IEnumerable<InstrumentViewModel> Instruments { get; set; }
		public InstrumentViewModel CreateViewModel { get; set; }
	}
}
