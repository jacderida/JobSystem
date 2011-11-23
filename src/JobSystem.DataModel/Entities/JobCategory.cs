using System;
namespace JobSystem.DataModel.Entities
{
	public class JobCategory
	{
		public virtual Guid Id { get; set; }
		public virtual string Description { get; set; }
	}
}