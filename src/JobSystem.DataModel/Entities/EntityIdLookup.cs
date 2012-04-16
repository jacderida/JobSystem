using System;

namespace JobSystem.DataModel.Entities
{
	public class EntityIdLookup
	{
		public virtual Guid Id { get; set; }
		public virtual string EntityTypeName { get; set; }
		public virtual int NextId { get; set; }
		public virtual string Suffix { get; set; }
		public virtual string Prefix { get; set; }
		public virtual int Version { get; set; }
	}
}