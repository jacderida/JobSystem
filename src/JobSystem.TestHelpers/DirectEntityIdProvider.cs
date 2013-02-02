using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;

namespace JobSystem.TestHelpers
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