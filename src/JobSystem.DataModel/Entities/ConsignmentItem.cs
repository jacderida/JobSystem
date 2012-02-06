using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class ConsignmentItem
	{
		public virtual Guid Id { get; set; }
		public virtual Consignment Consignment { get; set; }
		public virtual int ItemNo { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual string Instructions { get; set; }
	}
}