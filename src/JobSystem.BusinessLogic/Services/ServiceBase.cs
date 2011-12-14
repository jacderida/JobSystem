using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Validation.Core;

namespace JobSystem.BusinessLogic.Services
{
	public class ServiceBase
	{
		private readonly IUserContext _applicationContext;		
		protected readonly IQueueDispatcher<IMessage> _dispatcher;

		public ServiceBase(IUserContext applicationContext, IQueueDispatcher<IMessage> queueDispatcher)
		{
			Check.NotNull(applicationContext, "applicationContext");
			Check.NotNull(queueDispatcher, "queueDispatcher");
			_applicationContext = applicationContext;
			_dispatcher = queueDispatcher;
		}

		public UserAccount CurrentUser
		{
			get { return _applicationContext.GetCurrentUser(); }
		}

		public void Notify(IMessage message)
		{
			_dispatcher.Enqueue(message);
		}

		protected void ValidateAnnotatedObjectThrowOnFailure(object objectToValidate)
		{
			var result = new List<ValidationResult>();
			var context = new ValidationContext(objectToValidate, null, null);
			Validator.TryValidateObject(objectToValidate, context, result, true);
			if (result.Count > 0)
				throw new DomainValidationException(result);
		}
	}
}