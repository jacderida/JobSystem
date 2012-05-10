using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.JobItems;

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
		[Required(ErrorMessageResourceName = "SerialNoRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(50, ErrorMessageResourceName = "SerialNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string SerialNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AssetNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string AssetNo { get; set; }
		public virtual UserAccount CreatedUser { get; set; }
		public virtual ListItem InitialStatus { get; set; }
		public virtual ListItem Status { get; set; }
		public virtual ListItem Field { get; set; }
		public virtual int CalPeriod { get; set; }
		[StringLength(255, ErrorMessageResourceName = "InstructionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Instructions { get; set; }
		[StringLength(255, ErrorMessageResourceName = "AccessoriesTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Accessories { get; set; }
		public virtual bool IsReturned { get; set; }
		[StringLength(255, ErrorMessageResourceName = "ReturnReasonTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string ReturnReason { get; set; }
		public virtual bool IsCertProduced { get; set; }
		public virtual bool IsMarkedForInvoicing { get; set; }
		[StringLength(255, ErrorMessageResourceName = "CommentsTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Comments { get; set; }
		public virtual bool IsInvoiced { get; set; }
		public virtual DateTime ProjectedDeliveryDate { get; set; }
		public virtual bool IsMarkedForMonthlyInvoicing { get; set; }
		public virtual IList<ItemHistory> HistoryItems { get; set; }

		public JobItem()
		{
			HistoryItems = new List<ItemHistory>();
		}
	}
}