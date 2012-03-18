using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly DeliveryItemService _deliveryItemService;

		public DeliveryService(
			IUserContext applicationContext,
			IDeliveryRepository deliveryRepository,
			DeliveryItemService deliveryItemService,
			ICustomerRepository customerRepository,
			IEntityIdProvider entityIdProvider,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_deliveryRepository = deliveryRepository;
			_customerRepository = customerRepository;
			_entityIdProvider = entityIdProvider;
			_deliveryItemService = deliveryItemService;
		}

		public void CreateDeliveriesFromPendingItems()
		{
			DoCreateDeliveriesFromPendingItems(_deliveryItemService.GetPendingDeliveryItems());
		}

		public void CreateDeliveriesFromPendingItems(List<Guid> pendingItemIds)
		{
			DoCreateDeliveriesFromPendingItems(_deliveryItemService.GetPendingDeliveryItems(pendingItemIds));
		}

		public Delivery Create(Guid id, Guid customerId, string fao)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the pending item.");
			var delivery = new Delivery();
			delivery.Id = id;
			delivery.DeliveryNoteNumber = _entityIdProvider.GetIdFor<Delivery>();
			delivery.Customer = GetCustomer(customerId);
			delivery.CreatedBy = CurrentUser;
			delivery.DateCreated = AppDateTime.GetUtcNow();
			delivery.Fao = fao;
			ValidateAnnotatedObjectThrowOnFailure(delivery);
			_deliveryRepository.Create(delivery);
			return delivery;
		}

		public IEnumerable<Delivery> GetDeliveries()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _deliveryRepository.GetDeliveries();
		}

		private void DoCreateDeliveriesFromPendingItems(IEnumerable<PendingDeliveryItem> pendingItems)
		{
			var customerGroups = pendingItems.GroupBy(g => g.Customer.Id);
			foreach (var group in customerGroups)
			{
				var i = 0;
				var deliveryId = Guid.NewGuid();
				foreach (var item in group)
				{
					if (i++ == 0)
						Create(deliveryId, item.Customer.Id, String.Empty);
					_deliveryItemService.Create(Guid.NewGuid(), deliveryId, item.JobItem.Id, item.Notes);
					_deliveryItemService.DeletePendingDeliveryItem(item.Id);
				}
			}
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