using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Currencies
{
	public class CurrencyListViewModel
	{
		public IEnumerable<CurrencyViewModel> Currencies { get; set; }
		public CurrencyViewModel CreateViewModel { get; set; }
	}
}