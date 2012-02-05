using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class ConsignmentService : ServiceBase
	{
		private readonly IConsignmentRepository _consignmentRepository;
		private readonly ISupplierRepository _supplierRepository;
		private readonly IEntityIdProvider _entityIdProvider;

		public ConsignmentService(
			IUserContext applicationContext,
			IConsignmentRepository consignmentRepository,
			ISupplierRepository supplierRepository,
			IEntityIdProvider entityIdProvider,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_consignmentRepository = consignmentRepository;
			_supplierRepository = supplierRepository;
			_entityIdProvider = entityIdProvider;
		}
	}
}