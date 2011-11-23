using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class JobItemLocation
	{
		public virtual Guid Id { get; set; }
		public virtual string Description { get; set; }
	}
}