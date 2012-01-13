using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class ListItemRepository : RepositoryBase<ListItem>, IListItemRepository
	{
		public ListItem GetByName(string name)
		{
			return CurrentSession.Query<ListItem>().Where(li => li.Name == name).SingleOrDefault();
		}

		public IEnumerable<ListItem> GetAllByType(ListItemType type)
		{
			return CurrentSession.Query<ListItem>().Where(li => li.Type == type);
		}
	}
}