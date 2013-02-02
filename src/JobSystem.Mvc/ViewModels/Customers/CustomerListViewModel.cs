using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Customers
{
    public class CustomerListViewModel : PageViewModel
    {
        public IEnumerable<CustomerIndexViewModel> Customers { get; set; }
    }
}