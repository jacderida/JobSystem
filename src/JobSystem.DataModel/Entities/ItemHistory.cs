using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.JobItems;

namespace JobSystem.DataModel.Entities
{
	public class ItemHistory
	{
		public virtual Guid Id { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual int WorkTime { get; set; }
		public virtual int OverTime { get; set; }
		[StringLength(255, ErrorMessageResourceName = "ItemHistoryReportTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Report { get; set; }
		public virtual ListItem Status { get; set; }
		public virtual ListItem WorkType { get; set; }
		public virtual ListItem WorkLocation { get; set; }
		public virtual UserAccount User { get; set; }
	}
}