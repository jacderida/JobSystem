using System;
using System.Collections.Generic;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Job
	{
		public virtual Guid Id { get; set; }
		public virtual int JobNo { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual string Notes { get; set; }
		public virtual string Contact { get; set; }
		public virtual string Instructions { get; set; }
		public virtual string OrderNo { get; set; }
		public virtual string AdviceNo { get; set; }

		public virtual Status InitialStatus { get; set; }
		public virtual Status Status { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual JobCategory Category { get; set; }
		public virtual List<JobItem> JobItems { get; set; }

		public Job()
		{
			JobItems = new List<JobItem>();
		}
	}
}