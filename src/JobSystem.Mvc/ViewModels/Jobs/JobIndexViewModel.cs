using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Jobs
{
    public class JobIndexViewModel
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        // This is auto generated and won't be displayed till after the job has created.
        [Display(Name = "Job Number")]
        public string JobNumber { get; set; }
        [Display(Name = "Date Created")]
        // This is auto generated and won't be displayed till after the job has created.
        public string DateCreatedString { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        [Display(Name = "Booked By")]
        public string CreatedBy { get; set; }
    }
}
