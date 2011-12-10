using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Users
{
	public class UserAccountIndexViewModel
	{
		public IEnumerable<UserAccountListViewModel> Users { get; set; }
		public UserAccountViewModel CreateEditModel { get; set; }
	}
}