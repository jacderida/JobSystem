using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class TestStandardRepository : RepositoryBase<TestStandard>, ITestStandardRepository
	{
		public IEnumerable<TestStandard> GetTestStandards()
		{
			return CurrentSession.Query<TestStandard>();
		}
	}
}