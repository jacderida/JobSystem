using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Suppliers
{
    public class SupplierListViewModel : PageViewModel
    {
        public IEnumerable<SupplierIndexViewModel> Suppliers { get; set; }
    }
}