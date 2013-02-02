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

        public ListItem GetByType(ListItemType type)
        {
            return CurrentSession.Query<ListItem>().Where(li => li.Type == type).Single();
        }

        public IEnumerable<ListItem> GetAllByCategory(ListItemCategoryType category)
        {
            return CurrentSession.Query<ListItem>().Where(li => li.Category.Type == category);
        }
    }
}