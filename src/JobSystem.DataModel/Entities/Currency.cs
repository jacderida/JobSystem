using System;

namespace JobSystem.DataModel.Entities
{
	public class Currency
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}
}