using System.Web.Mvc;
using System.Collections.Generic;
using JobSystem.Mvc.ViewModels.Instruments;

namespace JobSystem.Mvc.ViewModels.Instruments
{
	public class InstrumentListViewModel
	{
		public IEnumerable<InstrumentViewModel> Instruments { get; set; }
		public InstrumentViewModel CreateViewModel { get; set; }
	}
}
