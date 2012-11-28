using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteListViewModel : PageViewModel
	{
		public IEnumerable<QuoteIndexViewModel> Items { get; set; }
	}
}