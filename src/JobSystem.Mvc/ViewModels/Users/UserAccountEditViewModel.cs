using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Users
{
	public class UserAccountEditViewModel
	{
		public Guid Id { get; set; }
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }
		[Required]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		[Display(Name = "Job Title")]
		public string JobTitle { get; set; }
	}
}