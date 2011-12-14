using System;

namespace JobSystem.Mvc.ViewModels.Users
{
	public class UserAccountListViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public string JobTitle { get; set; }
	}
}