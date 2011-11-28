using JobSystem.DataModel;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.DataModel.Entities;
using System;

namespace JobSystem.BusinessLogic.Services
{
	public class InstrumentService : ServiceBase
	{
		private readonly IInstrumentRepository _instrumentRepository;

		public InstrumentService(
			IUserContext applicationContext,
			IInstrumentRepository instrumentRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_instrumentRepository = instrumentRepository;
		}

		public Instrument Create(Guid id, string manufacturer, string modelNo, string range, string description)
		{
			throw new NotImplementedException();
		}
	}
}