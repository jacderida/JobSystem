using System;
using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuotePendingListViewModel : PageViewModel
	{
		public IEnumerable<QuoteItemIndexViewModel> QuoteItems;
		public Guid[] ToBeConvertedIds;
	}
}