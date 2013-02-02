using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.JobItems;

namespace JobSystem.Mvc.ViewModels.WorkItems
{
    public class WorkItemDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid JobItemId { get; set; }
        public int WorkTime { get; set; }
        public int OverTime { get; set; }
        [StringLength(255, ErrorMessageResourceName = "ItemHistoryReportTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public string Report { get; set; }
        public string Status { get; set; }
        public string WorkType { get; set; }
        public string WorkBy { get; set; }
        public string DateCreated { get; set; }
    }
}
