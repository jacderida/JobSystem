using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.QuoteItems;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class PendingQuoteItem
	{
		public virtual Guid Id { get; set; }
		[StringLength(50, ErrorMessageResourceName = "InvalidOrderNo", ErrorMessageResourceType = typeof(Messages))]
		public virtual string OrderNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "InvalidAdviceNo", ErrorMessageResourceType = typeof(Messages))]
		public virtual string AdviceNo { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual decimal Labour { get; set; }
		public virtual decimal Calibration { get; set; }
		public virtual decimal Parts { get; set; }
		public virtual decimal Carriage { get; set; }
		public virtual decimal Investigation { get; set; }
		public virtual int Days { get; set; }
		[StringLength(2000, ErrorMessageResourceName = "InvalidReport", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Report { get; set; }
		public virtual bool BeyondEconomicRepair { get; set; }
	}
}