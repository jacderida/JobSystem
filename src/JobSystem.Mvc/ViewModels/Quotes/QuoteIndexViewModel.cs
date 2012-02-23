using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteIndexViewModel
	{
		public string Id { get; set; }
		[Display(Name = "Order Number")]
		public string OrderNo { get; set; }
		[Display(Name = "Advice Number")]
		public string AdviceNo { get; set; }
	}
}