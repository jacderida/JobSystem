using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Invoices
{
    public class InvoiceItemListViewModel : PageViewModel
    {
        public IEnumerable<InvoiceItemIndexViewModel> Items { get; set; }
    }
}