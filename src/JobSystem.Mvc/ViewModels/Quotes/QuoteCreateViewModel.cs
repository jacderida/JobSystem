using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Quotes
{
    public class QuoteCreateViewModel
    {
        public Guid Id { get; set; }
        public Guid JobItemId { get; set; }
        public Guid JobId { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNo { get; set; }
        [Display(Name = "Advice Number")]
        public string AdviceNo { get; set; }
        public decimal Repair { get; set; }
        public decimal Calibration { get; set; }
        public decimal Parts { get; set; }
        public decimal Carriage { get; set; }
        public decimal Investigation { get; set; }
        public int Days { get; set; }
        [Display(Name = "Item BER")]
        public bool ItemBER { get; set; }
        public string Report { get; set; }
        [Display(Name = "Quote Individually")]
        public bool IsIndividual { get; set; }
        public Guid CurrencyId { get; set; }
        [Display(Name = "Currency")]
        public IEnumerable<SelectListItem> Currencies { get; set; }
    }
}