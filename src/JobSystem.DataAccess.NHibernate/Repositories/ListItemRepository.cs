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
		public IEnumerable<ListItem> GetAllByType(ListItemType type)
		{
			return CurrentSession.Query<ListItem>().Where(li => li.Type == type);
		}

		public IEnumerable<TaxCode> GetTaxCodes()
		{
			return CurrentSession.Query<TaxCode>();
		}

		public IEnumerable<BankDetails> GetBankDetails()
		{
			return CurrentSession.Query<BankDetails>();
		}
	}
}