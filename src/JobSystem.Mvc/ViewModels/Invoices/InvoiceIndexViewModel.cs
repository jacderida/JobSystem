using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Invoices
{
    public class InvoiceIndexViewModel
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public string InvoiceNo { get; set; }
        public string PaymentTerm { get; set; }
        public string TaxCode { get; set; }
        public string Currency { get; set; }
        public string CustomerName { get; set; }
    }
}