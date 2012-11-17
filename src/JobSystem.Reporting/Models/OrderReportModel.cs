using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.Reporting.Models
{
	public class OrderReportModel : SupplierReportModel
	{
		public string OrderNo { get; set; }
		public DateTime OrderDate { get; set; }
		public string Contact { get; set; }
		public string OrderInstructions { get; set; }
		public int ItemNo { get; set; }
		public string EquipmentDescription { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
		public int Days { get; set; }
		public string JobRef { get; set; }
		public string ItemInstructions { get; set; }
		public string PartNo { get; set; }
		public string PreparedBy { get; set; }
	}
}