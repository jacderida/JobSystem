using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Consignment
	{
		public virtual Guid Id { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual Supplier Supplier { get; set; }
		public virtual UserAccount CreatedBy { get; set; }
	}
}