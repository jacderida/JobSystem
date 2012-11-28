using System;
using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Consignments
{
	public class ConsignmentPendingListViewModel : PageViewModel
	{
		public IEnumerable<ConsignmentItemIndexViewModel> ConsignmentItems;
		public Guid[] ToBeConvertedIds;
	}
}