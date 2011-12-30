using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Jobs
{
	public class JobListViewModel
	{
		public IEnumerable<JobIndexViewModel> Jobs { get; set; }
		public JobCreateViewModel CreateViewModel { get; set; }
	}
}
