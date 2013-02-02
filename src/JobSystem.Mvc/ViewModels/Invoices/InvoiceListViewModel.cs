using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Invoices
{
    public class InvoiceListViewModel : PageViewModel
    {
        public IEnumerable<InvoiceIndexViewModel> Invoices { get; set; }
    }
}