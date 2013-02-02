using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface IListItemRepository : IReadWriteRepository<ListItem, Guid>
    {
        ListItem GetByName(string name);
        ListItem GetByType(ListItemType type);
        IEnumerable<ListItem> GetAllByCategory(ListItemCategoryType category);
    }
}