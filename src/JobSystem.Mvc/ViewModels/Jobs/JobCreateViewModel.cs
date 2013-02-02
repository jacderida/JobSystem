using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Jobs
{
    public class JobCreateViewModel
    {
        // This is auto generated and won't be displayed till after the job has created.
        [Display(Name = "Job Number")]
        public int JobNumber { get; set; }
        [Display(Name = "Date Created")]
        // This is auto generated and won't be displayed till after the job has created.
        public DateTime DateCreated { get; set; }
        // Status only applies to job items.
        [Display(Name = "Status")]
        public string Status { get; set; }
        // Changed to string.
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        // Changed to string.
        [Display(Name = "Advice Number")]
        public string AdviceNumber { get; set; }
        [Display(Name = "Contact")]
        public string Contact { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
        [Display(Name = "Customer")]
        public Guid CustomerId { get; set; }
        public string CreatedBy { get; set; }
        public Guid TypeId { get; set; }
        [Display(Name = "Job Type")]
        public IEnumerable<SelectListItem> JobTypes { get; set; }
        public AttachmentViewModel[] Attachments { get; set; }
    }
}