using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteCreateViewModel
	{
		public Guid Id { get; set; }
		[Display(Name = "Order Number")]
		public string OrderNo { get; set; }
		[Display(Name = "Advice Number")]
		public string AdviceNo { get; set; }
		public string Repair { get; set; }
		public string Calibration { get; set; }
		public string Parts { get; set; }
		public string Carriage { get; set; }
		public string Investigation { get; set; }
		public string Days { get; set; }
		[Display(Name = "Item BER")]
		public bool ItemBER { get; set; }
		public string Report { get; set; }
		[Display(Name = "Quote Individually")]
		public bool IsIndividual { get; set; }
	}
}