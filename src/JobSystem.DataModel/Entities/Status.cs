using System;

namespace JobSystem.DataModel.Entities
{
	public class Status
	{
		public virtual Guid Id { get; set; }
		public virtual string Description { get; set; }
	}
}