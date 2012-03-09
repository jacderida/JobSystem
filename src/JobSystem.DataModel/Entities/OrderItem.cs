using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class OrderItem
	{
		public virtual Guid Id { get; set; }
		public virtual Order Order { get; set; }
		public virtual int ItemNo { get; set; }
		public virtual int Quantity { get; set; }
		public virtual string PartNo { get; set; }
		public virtual string Instructions { get; set; }
		public virtual int DeliveryDays { get; set; }
		public virtual DateTime? DateReceived { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual decimal Price { get; set; }
		public virtual bool InvoiceReceived { get; set; }
	}
}