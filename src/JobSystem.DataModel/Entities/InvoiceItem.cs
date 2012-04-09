using System;

namespace JobSystem.DataModel.Entities
{
	public class InvoiceItem
	{
		public virtual Guid Id { get; set; }
		public virtual Invoice Invoice { get; set; }
		public virtual int ItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual decimal Price { get; set; }
		public virtual decimal CalibrationPrice { get; set; }
		public virtual decimal RepairPrice { get; set; }
		public virtual decimal PartsPrice { get; set; }
		public virtual decimal CarriagePrice { get; set; }
		public virtual decimal InvestigationPrice { get; set; }
		public virtual JobItem JobItem { get; set; }
	}
}