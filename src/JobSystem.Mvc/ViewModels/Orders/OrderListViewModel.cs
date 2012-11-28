using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Orders
{
	public class OrderListViewModel : PageViewModel
	{
		public IEnumerable<OrderIndexViewModel> Items { get; set; }
	}
}