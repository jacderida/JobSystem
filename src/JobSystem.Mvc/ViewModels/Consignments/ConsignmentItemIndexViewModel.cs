using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Consignments
{
	public class ConsignmentItemIndexViewModel
	{
		public Guid Id { get; set; }
		public string SupplierId { get; set; }
		public string SupplierName { get; set; }
		public string Instructions { get; set; }
		public Guid JobItemId { get; set; }
		public bool ToBeConverted { get; set; }
		public string InstrumentDetails { get; set; }
	}
}