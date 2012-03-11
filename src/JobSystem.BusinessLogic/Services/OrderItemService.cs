using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Resources.Orders;
using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Services
{
	public class OrderItemService : ServiceBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly ISupplierRepository _supplierRepository;
		private readonly IJobItemRepository _jobItemRepository;
		private readonly IListItemRepository _listItemRepository;

		public OrderItemService(
			IUserContext applicationContext,
			IOrderRepository orderRepository,
			IOrderItemRepository orderItemRepository,
			ISupplierRepository supplierRepository,
			IJobItemRepository jobItemRepository,
			IListItemRepository listItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_orderRepository = orderRepository;
			_orderItemRepository = orderItemRepository;
			_supplierRepository = supplierRepository;
			_jobItemRepository = jobItemRepository;
			_listItemRepository = listItemRepository;
		}

		public OrderItem Create(Guid id, Guid orderId, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price)
		{
			return DoCreateOrderItem(id, orderId, quantity, partNo, instructions, deliveryDays, jobItemId, price, ListItemType.StatusAwaitingParts);
		}

		public OrderItem Edit(Guid id, int quantity, string partNo, string instructions, int deliveryDays, decimal price)
		{
			var orderItem = GetById(id);
			orderItem.Quantity = GetQuantity(quantity);
			orderItem.PartNo = partNo;
			orderItem.Instructions = instructions;
			orderItem.DeliveryDays = GetDeliveryDays(deliveryDays);
			orderItem.Price = GetPrice(price);
			ValidateAnnotatedObjectThrowOnFailure(orderItem);
			_orderItemRepository.Update(orderItem);
			return orderItem;
		}

		public PendingOrderItem CreatePending(Guid id, Guid supplierId, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("A valid ID must be supplied for the pending item");
			if (_orderItemRepository.JobItemHasPendingOrderItem(jobItemId))
				throw new DomainValidationException(OrderItemMessages.PendingItemExists, "JobItemId");
			var pendingItem = new PendingOrderItem();
			pendingItem.Id = id;
			pendingItem.Supplier = GetSupplier(supplierId);
			pendingItem.Quantity = GetQuantity(quantity);
			pendingItem.PartNo = partNo;
			pendingItem.Instructions = instructions;
			pendingItem.DeliveryDays = GetDeliveryDays(deliveryDays);
			pendingItem.JobItem = GetJobItem(jobItemId);
			pendingItem.Price = GetPrice(price);
			ValidateAnnotatedObjectThrowOnFailure(pendingItem);
			_orderItemRepository.CreatePending(pendingItem);
			return pendingItem;
		}

		public PendingOrderItem EditPending(
			Guid id, Guid supplierId, int quantity, string partNo, string instructions, int deliveryDays, decimal price)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			var pendingItem = _orderItemRepository.GetPendingOrderItem(id);
			if (pendingItem == null)
				throw new ArgumentException("A valid ID must be supplied for the pending item");
			pendingItem.Supplier = GetSupplier(supplierId);
			pendingItem.Quantity = GetQuantity(quantity);
			pendingItem.PartNo = partNo;
			pendingItem.Instructions = instructions;
			pendingItem.DeliveryDays = GetDeliveryDays(deliveryDays);
			pendingItem.Price = GetPrice(price);
			ValidateAnnotatedObjectThrowOnFailure(pendingItem);
			_orderItemRepository.UpdatePendingItem(pendingItem);
			return pendingItem;
		}

		public OrderItem GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			var orderItem = _orderItemRepository.GetById(id);
			if (orderItem == null)
				throw new ArgumentException("A valid ID must be supplied for the order item");
			return orderItem;
		}

		public IEnumerable<OrderItem> GetOrderItems(Guid orderId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			return _orderItemRepository.GetOrderItems(orderId);
		}

		public IEnumerable<OrderItem> GetOrderItemsForJobItem(Guid jobItemId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			return _orderItemRepository.GetOrderItemsForJobItem(jobItemId);
		}

		public PendingOrderItem GetPendingOrderItemForJobItem(Guid jobItemId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			var pendingItem = _orderItemRepository.GetPendingOrderItemForJobItem(jobItemId);
			if (pendingItem == null)
				throw new ArgumentException("A valid ID must be supplied for the job item");
			return pendingItem;
		}

		public void DeletePendingItem(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			_orderItemRepository.DeletePendingItem(id);
		}

		public IEnumerable<PendingOrderItem> GetPendingOrderItems()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			return _orderItemRepository.GetPendingOrderItems();
		}

		public IEnumerable<PendingOrderItem> GetPendingOrderItems(IList<Guid> pendingItemIds)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			return _orderItemRepository.GetPendingOrderItems(pendingItemIds);
		}

		private OrderItem DoCreateOrderItem(
			Guid id, Guid orderId, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price, ListItemType jobItemStatusType)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(OrderItemMessages.InsufficientSecurity, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("A valid ID must be supplied for the order item");
			var order = GetOrder(orderId);
			var orderItem = new OrderItem();
			orderItem.Id = id;
			orderItem.Order = order;
			orderItem.ItemNo = order.Items.Count + 1;
			orderItem.Quantity = GetQuantity(quantity);
			orderItem.PartNo = partNo;
			orderItem.Instructions = instructions;
			orderItem.DeliveryDays = GetDeliveryDays(deliveryDays);
			orderItem.Price = GetPrice(price);
			ValidateAnnotatedObjectThrowOnFailure(orderItem);
			if (jobItemId != Guid.Empty)
			{
				var jobItem = GetJobItem(jobItemId);
				jobItem.Status = _listItemRepository.GetByType(jobItemStatusType);
				_jobItemRepository.Update(jobItem);
				_jobItemRepository.EmitItemHistory(
					CurrentUser, jobItem.Id, 0, 0, "Item on order OR2000", jobItemStatusType, ListItemType.WorkTypeAdministration, jobItem.Location.Type);
				orderItem.JobItem = jobItem;
			}
			_orderItemRepository.Create(orderItem);
			return orderItem;
		}

		private Supplier GetSupplier(Guid supplierId)
		{
			var supplier = _supplierRepository.GetById(supplierId);
			if (supplier == null)
				throw new ArgumentException("A valid ID must be supplied for the supplier");
			return supplier;
		}

		private Order GetOrder(Guid orderId)
		{
			var order = _orderRepository.GetById(orderId);
			if (order == null)
				throw new ArgumentException("A valid ID must be supplied for the order");
			return order;
		}

		private int GetQuantity(int quantity)
		{
			if (quantity < 1)
				throw new DomainValidationException(OrderItemMessages.InvalidQuantity, "Quantity");
			return quantity;
		}

		private int GetDeliveryDays(int deliveryDays)
		{
			if (deliveryDays < 0)
				throw new DomainValidationException(OrderItemMessages.InvalidDeliveryDays, "DeliveryDays");
			return deliveryDays;
		}

		private decimal GetPrice(decimal price)
		{
			if (price < 0)
				throw new DomainValidationException(OrderItemMessages.InvalidPrice, "Price");
			return price;
		}

		private JobItem GetJobItem(Guid jobItemId)
		{
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid ID must be supplied for the job item");
			if (jobItem.Job.IsPending)
				throw new DomainValidationException(OrderItemMessages.JobPending, "JobItem");
			return jobItem;
		}
	}
}