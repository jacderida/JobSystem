﻿using System.Collections.Generic;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using System;

namespace JobSystem.BusinessLogic.Services
{
    public class ListItemService : ServiceBase
    {
        private readonly IListItemRepository _listItemRepository;

        public ListItemService(
            IUserContext applicationContext,
            IListItemRepository listItemRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _listItemRepository = listItemRepository;
        }

        public ListItem GetByName(string name)
        {
            return _listItemRepository.GetByName(name);
        }

        public ListItem GetByType(ListItemType type)
        {
            return _listItemRepository.GetByType(type);
        }

        public ListItem GetById(Guid id)
        {
            var listItem = _listItemRepository.GetById(id);
            if (listItem == null)
                throw new ArgumentException("A valid ID must be supplied for the list item ID");
            return _listItemRepository.GetById(id);
        }

        public IEnumerable<ListItem> GetAllByCategory(ListItemCategoryType category)
        {
            return _listItemRepository.GetAllByCategory(category);
        }
    }
}