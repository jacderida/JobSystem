using System;
using System.Collections.Generic;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class JobItem
	{
		public virtual Guid Id { get; set; }
		public virtual Job Job { get; set; }
		public virtual int ItemNo { get; set; }
		public virtual DateTime Created { get; set; }
		public virtual Instrument Instrument { get; set; }
		public virtual string SerialNo { get; set; }
		public virtual string AssetNo { get; set; }
		public virtual UserAccount CreatedUser { get; set; }
		public virtual ListItem InitialStatus { get; set; }
		public virtual ListItem Status { get; set; }
		public virtual ListItem Location { get; set; }
		public virtual ListItem Field { get; set; }
		public virtual int CalPeriod { get; set; }
		public virtual string Instructions { get; set; }
		public virtual string Accessories { get; set; }
		public virtual bool IsReturned { get; set; }
		public virtual string ReturnReason { get; set; }
		public virtual bool IsCertProduced { get; set; }
		public virtual bool IsMarkedForInvoicing { get; set; }
		public virtual string Comments { get; set; }
		public virtual bool IsInvoiced { get; set; }
		public virtual DateTime ProjectedDeliveryDate { get; set; }
		public virtual bool IsMarkedForMonthlyInvoicing { get; set; }
		public virtual List<ItemHistory> HistoryItems { get; set; }

		public JobItem()
		{
			HistoryItems = new List<ItemHistory>();
		}
	}
}