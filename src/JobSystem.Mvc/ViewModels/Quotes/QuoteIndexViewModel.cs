using System;
using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteIndexViewModel
	{
		public Guid Id { get; set; }
		public string QuoteNo { get; set; }
		public string CreatedBy { get; set; }
		public string DateCreated { get; set; }
		public string CustomerName { get; set; }
		public string OrderNo { get; set; }
		public string AdviceNo { get; set; }
		public List<QuoteItemIndexViewModel> QuoteItems { get; set; }
		public string CurrencyName { get; set; }
		public int ItemCount { get; set; }
	}
}