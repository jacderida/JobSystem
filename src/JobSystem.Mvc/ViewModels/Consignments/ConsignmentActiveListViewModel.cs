using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Consignments
{
	public class ConsignmentActiveListViewModel : PageViewModel
	{
		public IEnumerable<ConsignmentIndexViewModel> Consignments;
	}
}