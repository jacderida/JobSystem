using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class PendingDeliveryItem
	{
		public virtual Guid Id { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual QuoteItem QuoteItem { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual string Notes { get; set; }
		public virtual bool BeyondEconomicRepair { get; set; }
	}
}