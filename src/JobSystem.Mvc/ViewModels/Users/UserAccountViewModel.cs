using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Users
{
	public class UserAccountViewModel
	{
		public Guid Id { get; set; }
		[Display(Name = "Name")]
		public string Name { get; set; }
		[Required]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }
		[Display(Name = "Job Title")]
		public string JobTitle { get; set; }
	}
}