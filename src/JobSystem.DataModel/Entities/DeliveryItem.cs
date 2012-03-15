using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class DeliveryItem
	{
		public Guid Id { get; set; }
		public int ItemNo { get; set; }
		public JobItem JobItem { get; set; }
		public QuoteItem QuoteItem { get; set; }
		public string Notes { get; set; }
		public string BeyondEconomicRepair { get; set; }
	}
}