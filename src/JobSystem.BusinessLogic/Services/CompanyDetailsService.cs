using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class CompanyDetailsService : ServiceBase
	{
		public CompanyDetailsService(
			IUserContext applicationContext,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
		}
	}
}