using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Users
{
	public class UserAccountViewModel
	{
		public Guid Id { get; set; }
		[Required]
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
		[Required]
		public string JobTitle { get; set; }
		[Display(Name = "Role")]
		public IEnumerable<SelectListItem> Roles { get; set; }
		public int RoleId { get; set; }
	}
}