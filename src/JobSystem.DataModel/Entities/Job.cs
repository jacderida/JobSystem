using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Jobs;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Job
	{
		public virtual Guid Id { get; set; }
		public virtual string JobNo { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual UserAccount CreatedBy { get; set; }
		[StringLength(2000, ErrorMessageResourceName = "NotesTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Notes { get; set; }
		[StringLength(50, ErrorMessageResourceName = "ContactTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Contact { get; set; }
		[StringLength(2000, ErrorMessageResourceName = "InstructionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Instructions { get; set; }
		[StringLength(50, ErrorMessageResourceName = "OrderNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string OrderNo { get; set; }
		[StringLength(50, ErrorMessageResourceName = "AdviceNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string AdviceNo { get; set; }
		public virtual ListItem Type { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual IList<JobItem> JobItems { get; set; }
		public virtual bool IsPending { get; set; }

		public Job()
		{
			JobItems = new List<JobItem>();
		}
	}
}