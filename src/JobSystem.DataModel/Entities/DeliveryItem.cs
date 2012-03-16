using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class DeliveryItem
	{
		public virtual Guid Id { get; set; }
		public virtual int ItemNo { get; set; }
		public virtual Delivery Delivery { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual QuoteItem QuoteItem { get; set; }
		public virtual string Notes { get; set; }
		public virtual string BeyondEconomicRepair { get; set; }
	}
}