using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.DataAccess.NHibernate.Repositories;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	public class DirectEntityIdProvider : IEntityIdProvider
	{
		private IEntityIdLookupRepository _entityIdLookupRepository = new EntityIdLookupRepository();

		public string GetIdFor<T>()
		{
			return _entityIdLookupRepository.GetNextId(typeof(T).ToString());
		}
	}
}