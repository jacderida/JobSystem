using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Users
{
	public class UserAccountViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string JobTitle { get; set; }
	}
}