using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.JobItems
{
	public class JobItemSearchResultsViewModel
	{
		public int ItemNo { get; set; }
		public string JobItemRef { get; set; }
		public Guid JobId { get; set; }
		public string JobNumber { get; set; }
		public string Instrument { get; set; }
		public string SerialNo { get; set; }
	}
}