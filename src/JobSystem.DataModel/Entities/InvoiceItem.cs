using System;

namespace JobSystem.DataModel.Entities
{
	public class InvoiceItem
	{
		public Guid Id { get; set; }
		public Invoice Invoice { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public decimal CalibrationPrice { get; set; }
		public decimal RepairPrice { get; set; }
		public decimal PartsPrice { get; set; }
		public decimal CarriagePrice { get; set; }
		public decimal InvestigationPrice { get; set; }
		public JobItem JobItem { get; set; }
	}
}