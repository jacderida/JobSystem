using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class QuoteItem
	{
		public virtual Guid Id { get; set; }
		public virtual Quote Quote { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual DateTime DateAccepted { get; set; }
		public virtual DateTime DateRejected { get; set; }
		public virtual decimal Labour { get; set; }
		public virtual decimal Calibration { get; set; }
		public virtual decimal Parts { get; set; }
		public virtual decimal Carriage { get; set; }
		public virtual decimal Investigation { get; set; }
		public virtual string Report { get; set; }
		public virtual ListItem Status { get; set; }
		public virtual int Days { get; set; }
		public virtual bool BeyondEconomicRepair { get; set; }
	}
}