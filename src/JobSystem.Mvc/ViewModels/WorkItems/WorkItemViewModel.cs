using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.JobItems;

namespace JobSystem.Mvc.ViewModels.WorkItems
{
	public class WorkItemViewModel
	{
		public Guid Id { get; set; }
		public Guid JobItemId { get; set; }
		public int WorkTime { get; set; }
		public int OverTime { get; set; }
		[StringLength(255, ErrorMessageResourceName = "ItemHistoryReportTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public string Report { get; set; }
		[Display(Name = "Status")]
		public IEnumerable<SelectListItem> Status { get; set; }
		public IEnumerable<SelectListItem> WorkType { get; set; }
		public IEnumerable<SelectListItem> WorkLocation { get; set; }
		public Guid StatusId { get; set; }
		public Guid WorkTypeId { get; set; }
		public Guid WorkLocationId { get; set; }
	}
}
