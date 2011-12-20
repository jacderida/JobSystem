using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IListItemRepository : IReadWriteRepository<ListItem, Guid>
	{
		IEnumerable<ListItem> GetAllByType(ListItemType type);
		IEnumerable<TaxCode> GetTaxCodes();
		IEnumerable<BankDetails> GetBankDetails();
	}
}