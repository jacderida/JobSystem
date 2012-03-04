using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteIndexViewModel
	{
		[Display(Name = "Order Number")]
		public string OrderNo { get; set; }
		[Display(Name = "Advice Number")]
		public string AdviceNo { get; set; }
		public List<QuoteItemIndexViewModel> QuoteItems { get; set; }
	}
}