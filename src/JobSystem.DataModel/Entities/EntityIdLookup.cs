using System;

namespace JobSystem.DataModel.Entities
{
	/// <summary>
	/// Object for holding information regarding the current Id for each type of entity.
	/// </summary>
	public class EntityIdLookup
	{
		public virtual Guid Id { get; set; }
		public virtual string EntityTypeName { get; set; }
		public virtual int NextId { get; set; }
		public virtual string Suffix { get; set; }
		public virtual string Prefix { get; set; }
	}
}