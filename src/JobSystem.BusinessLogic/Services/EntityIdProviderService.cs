using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
    public class EntityIdProviderService : ServiceBase
    {
        private IEntityIdLookupRepository _entityIdLookupRepository;

        public EntityIdProviderService(
            IUserContext applicationContext,
            IEntityIdLookupRepository entityIdLookupRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _entityIdLookupRepository = entityIdLookupRepository;
        }

        public string GetIdFor(string typeName)
        {
            return _entityIdLookupRepository.GetNextId(typeName);
        }
    }
}