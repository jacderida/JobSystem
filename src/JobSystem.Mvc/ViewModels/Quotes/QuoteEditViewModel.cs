using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Quotes
{
    public class QuoteEditViewModel
    {
        public Guid Id { get; set; }
        public Guid JobItemId { get; set; }
        public Guid JobId { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNo { get; set; }
        [Display(Name = "Advice Number")]
        public string AdviceNo { get; set; }
        [Display(Name = "Currency")]
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public Guid CurrencyId { get; set; }
    }
}