using System;
using System.Collections.Generic;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class ListItemCategory
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ListItemCategoryType Type { get; set; }
		public virtual IList<ListItem> ListItems { get; set; }

		public ListItemCategory()
		{
			ListItems = new List<ListItem>();
		}
	}
}