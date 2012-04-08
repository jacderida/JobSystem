using System;

namespace JobSystem.DataModel.Entities
{
	public class PendingInvoiceItem
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public decimal CalibrationPrice { get; set; }
		public decimal RepairPrice { get; set; }
		public decimal PartsPrice { get; set; }
		public decimal CarriagePrice { get; set; }
		public decimal InvestigationPrice { get; set; }
		public JobItem JobItem { get; set; }
		public string OrderNo { get; set; }
	}
}