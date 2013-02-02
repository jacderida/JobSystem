using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Jobs
{
    public class JobEditViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        [Display(Name = "Advice Number")]
        public string AdviceNumber { get; set; }
        [Display(Name = "Contact")]
        public string Contact { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
    }
}