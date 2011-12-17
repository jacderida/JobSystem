using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Jobs
{
    public class JobCreateViewModel
    {
       	[Display(Name = "Job Number")]
		public int JobNumber {get; set;}
		[Display(Name = "Date Created")]
		public DateTime DateCreated {get; set;}
		[Display(Name = "Status")]
		public string Status {get; set;}
		[Display(Name = "Order Number")]
		public int OrderNumber {get; set;}
		[Display(Name = "Advice Number")]
		public int AdviceNumber {get; set;}
		[Display(Name = "Contact")]
		public string Contact {get; set;}
		[Display(Name = "Job Note")]
		public string JobNote {get; set;}
		[Display(Name = "Instructions")]
		public string Instructions {get; set;}
		[Display(Name = "Customer")]
		public string Customer { get; set; }
    }
}

