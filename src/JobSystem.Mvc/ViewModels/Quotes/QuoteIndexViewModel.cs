using System;
using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteIndexViewModel
	{
		public Guid id { get; set; }
		public List<QuoteItemIndexViewModel> QuoteItems { get; set; }
	}
}