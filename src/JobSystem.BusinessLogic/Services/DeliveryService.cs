using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Delivery;

namespace JobSystem.BusinessLogic.Services
{
	public class DeliveryService : ServiceBase
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IDeliveryRepository _deliveryRepository;

		public DeliveryService(
			IUserContext applicationContext,
			IDeliveryRepository deliveryRepository,
			ICustomerRepository customerRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_deliveryRepository = deliveryRepository;
			_customerRepository = customerRepository;
		}

		public Delivery Create(Guid id, Guid customerId, string fao)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the pending item.");
			var delivery = new Delivery();
			delivery.Id = id;
			delivery.Customer = GetCustomer(customerId);
			delivery.CreatedBy = CurrentUser;
			delivery.DateCreated = AppDateTime.GetUtcNow();
			delivery.Fao = fao;
			ValidateAnnotatedObjectThrowOnFailure(delivery);
			_deliveryRepository.Create(delivery);
			return delivery;
		}

		private Customer GetCustomer(Guid customerId)
		{
			var customer = _customerRepository.GetById(customerId);
			if (customer == null)
				throw new ArgumentException("A valid ID must be supplied for the customer");
			return customer;
		}
	}
}