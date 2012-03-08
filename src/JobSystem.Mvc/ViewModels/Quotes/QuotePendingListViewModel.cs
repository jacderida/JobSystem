using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuotePendingListViewModel
	{
		public List<QuoteItemIndexViewModel> QuoteItems;
		public Guid[] ToBeConvertedIds;
	}
}