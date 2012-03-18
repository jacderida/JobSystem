using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Resources.Delivery;

namespace JobSystem.BusinessLogic.Services
{
	public class DeliveryItemService : ServiceBase
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IDeliveryRepository _deliveryRepository;
		private readonly IDeliveryItemRepository _deliveryItemRepository;
		private readonly IJobItemRepository _jobItemRepository;
		private readonly IQuoteItemRepository _quoteItemRepository;
		private readonly IListItemRepository _listItemRepository;

		public DeliveryItemService(
			IUserContext applicationContext,
			IDeliveryRepository deliveryRepository,
			IDeliveryItemRepository deliveryItemRepository,
			IJobItemRepository jobItemRepository,
			IQuoteItemRepository quoteItemRepository,
			IListItemRepository listItemRepository,
			ICustomerRepository customerRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_deliveryRepository = deliveryRepository;
			_deliveryItemRepository = deliveryItemRepository;
			_jobItemRepository = jobItemRepository;
			_quoteItemRepository = quoteItemRepository;
			_listItemRepository = listItemRepository;
			_customerRepository = customerRepository;
		}

		public DeliveryItem Create(Guid id, Guid deliveryId, Guid jobItemId, string notes)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(DeliveryItemMessages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the pending item.");
			var deliveryItem = new DeliveryItem();
			deliveryItem.Id = id;
			deliveryItem.ItemNo = _deliveryRepository.GetDeliveryItemCount(deliveryId) + 1;
			deliveryItem.Delivery = GetDelivery(deliveryId);
			var jobItem = GetJobItem(jobItemId);
			deliveryItem.JobItem = jobItem;
			deliveryItem.Notes = notes;
			deliveryItem.QuoteItem = GetQuoteItem(jobItemId);
			ValidateAnnotatedObjectThrowOnFailure(deliveryItem);
			jobItem.Status = _listItemRepository.GetByType(ListItemType.StatusDeliveryNoteProduced);
			jobItem.Location = _listItemRepository.GetByType(ListItemType.WorkLocationCompleted);
			_jobItemRepository.EmitItemHistory(
				CurrentUser, jobItem.Id, 0, 0, "Item added to delivery note DR2000", ListItemType.StatusDeliveryNoteProduced, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationCompleted);
			_jobItemRepository.Update(jobItem);
			_deliveryItemRepository.Create(deliveryItem);
			return deliveryItem;
		}

		public PendingDeliveryItem CreatePending(Guid id, Guid jobItemId, Guid customerId, string notes)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(DeliveryItemMessages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the pending item.");
			var pendingItem = new PendingDeliveryItem();
			pendingItem.Id = id;
			pendingItem.JobItem = GetJobItem(jobItemId);
			if (_quoteItemRepository.JobItemHasPendingQuoteItem(jobItemId))
				throw new DomainValidationException(DeliveryItemMessages.PendingItemExists, "JobItemId");
			pendingItem.Customer = GetCustomer(customerId);
			pendingItem.QuoteItem = GetQuoteItem(jobItemId);
			pendingItem.Notes = notes;
			ValidateAnnotatedObjectThrowOnFailure(pendingItem);
			_deliveryItemRepository.CreatePending(pendingItem);
			return pendingItem;
		}

		private Delivery GetDelivery(Guid deliveryId)
		{
			var delivery = _deliveryRepository.GetById(deliveryId);
			if (delivery == null)
				throw new ArgumentException("A valid delivery ID must be supplied for the item");
			return delivery;
		}

		private Customer GetCustomer(Guid customerId)
		{
			var customer = _customerRepository.GetById(customerId);
			if (customer == null)
				throw new ArgumentException("A valid ID must be passed for the customer");
			return customer;
		}

		private JobItem GetJobItem(Guid jobItemId)
		{
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid job item ID must be supplied for the pending item.");
			if (jobItem.Job.IsPending)
				throw new DomainValidationException(DeliveryItemMessages.JobPending, "JobItemId");
			return jobItem;
		}

		private QuoteItem GetQuoteItem(Guid jobItemId)
		{
			return _quoteItemRepository.GetQuoteItemForJobItem(jobItemId);
		}
	}
}