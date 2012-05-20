using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Currencies
{
	public class CurrencyViewModel
	{
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[Display(Name = "Display Message")]
		public string DisplayMessage { get; set; }
	}
}