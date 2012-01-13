using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class JobItemRepository : RepositoryBase<JobItem>, IJobItemRepository
	{
	}
}