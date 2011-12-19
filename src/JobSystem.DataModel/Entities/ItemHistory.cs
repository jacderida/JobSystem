using System;

namespace JobSystem.DataModel.Entities
{
	public class ItemHistory
	{
		public virtual Guid Id { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual int WorkTypeId { get; set; }
		public virtual int WorkTime { get; set; }
		public virtual int OverTime { get; set; }
		public virtual string Report { get; set; }
		public virtual ListItem Status { get; set; }
		public virtual ListItem WorkLocation { get; set; }
		public virtual UserAccount User { get; set; }
	}
}