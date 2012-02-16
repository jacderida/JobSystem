using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Consignments
{
	public class ConsignmentPendingListViewModel
	{
		public List<ConsignmentItemIndexViewModel> ConsignmentItems;
		public Guid[] ToBeConvertedIds;
	}
}