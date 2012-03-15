using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.DataModel.Entities
{
	public class PendingDeliveryItem
	{
		public Guid Id { get; set; }
		public JobItem JobItem { get; set; }
		public QuoteItem QuoteItem { get; set; }
		public Customer Customer { get; set; }
		public bool BeyondEconomicRepair { get; set; }
	}
}