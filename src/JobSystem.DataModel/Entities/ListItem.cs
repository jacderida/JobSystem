using System;

namespace JobSystem.DataModel.Entities
{
	public class ListItem
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ListItemType Type { get; set; }
	}
}