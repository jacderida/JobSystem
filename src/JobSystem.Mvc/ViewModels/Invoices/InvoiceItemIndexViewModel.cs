using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Invoices
{
	public class InvoiceItemIndexViewModel
	{
		public Guid Id { get; set; }
		public string JobItemRef { get; set; }
		public double CalibrationPrice { get; set; }
		public double CarriagePrice { get; set; }
		public double PartsPrice { get; set; }
		public double RepairPrice { get; set; }
		public double InvestigationPrice { get; set; }
		public string Description { get; set; }
		public string OrderNo { get; set; }
	}
}