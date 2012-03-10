using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class PendingOrderItem
	{
		public virtual Guid Id { get; set; }
		public virtual Supplier Supplier { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual int Quantity { get; set; }
		public virtual string PartNo { get; set; }
		public virtual string Instructions { get; set; }
		public virtual int DeliveryDays { get; set; }
		public virtual decimal Price { get; set; }
	}
}