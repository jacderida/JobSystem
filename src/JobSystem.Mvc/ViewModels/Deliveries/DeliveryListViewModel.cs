using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Deliveries
{
    public class DeliveryListViewModel : PageViewModel
    {
        public IEnumerable<DeliveryIndexViewModel> Items { get; set; }
    }
}