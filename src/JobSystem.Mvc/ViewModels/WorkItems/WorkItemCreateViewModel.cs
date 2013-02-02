using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.JobItems;

namespace JobSystem.Mvc.ViewModels.WorkItems
{
    public class WorkItemCreateViewModel
    {
        public Guid Id { get; set; }
        public Guid JobItemId { get; set; }
        [Required]
        [Display(Name = "Work Time")]
        public int WorkTime { get; set; }
        [Required]
        [Display(Name = "Over Time")]
        public int OverTime { get; set; }
        [StringLength(255, ErrorMessageResourceName = "ItemHistoryReportTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public string Report { get; set; }
        [Display(Name = "Status")]
        public IEnumerable<SelectListItem> Status { get; set; }
        [Display(Name = "Work Type")]
        public IEnumerable<SelectListItem> WorkType { get; set; }
        public Guid StatusId { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
