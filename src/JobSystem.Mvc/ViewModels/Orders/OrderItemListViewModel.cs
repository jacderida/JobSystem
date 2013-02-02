using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Orders
{
    public class OrderItemListViewModel : PageViewModel
    {
        public IEnumerable<OrderItemIndexViewModel> Items { get; set; }
    }
}