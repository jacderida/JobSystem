using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Mvc.ViewModels.JobItems;

namespace JobSystem.Mvc.ViewModels.Jobs
{
	public class JobDetailsViewModel
	{
		public string Id { get; set; }
		// This is auto generated and won't be displayed till after the job has created.
		[Display(Name = "Job Number")]
		public string JobNumber { get; set; }
		[Display(Name = "Date Created")]
		// This is auto generated and won't be displayed till after the job has created.
		public string DateCreated { get; set; }
		[Display(Name = "Order Number")]
		public string OrderNumber { get; set; }
		[Display(Name = "Booked By")]
		public string Instruction { get; set; }
		public string Note { get; set; }
		public string CreatedBy { get; set; }
		public string AdviceNumber { get; set; }
		public string Contact { get; set; }
		public string CustomerName { get; set; }
		public string CustomerAddress1 { get; set; }
		public string CustomerAddress2 { get; set; }
		public string CustomerAddress3 { get; set; }
		public string CustomerAddress4 { get; set; }
		public string CustomerAddress5 { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerTelephone { get; set; }
		public bool IsPending { get; set; }
		public IList<JobItemViewModel> JobItems { get; set; }
	}
}
