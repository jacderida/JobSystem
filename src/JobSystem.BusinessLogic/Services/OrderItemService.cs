using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Resources.Orders;

namespace JobSystem.BusinessLogic.Services
{
	public class OrderItemService : ServiceBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly IJobItemRepository _jobItemRepository;
		private readonly IListItemRepository _listItemRepository;

		public OrderItemService(
			IUserContext applicationContext,
			IOrderRepository orderRepository,
			IOrderItemRepository orderItemRepository,
			IJobItemRepository jobItemRepository,
			IListItemRepository listItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_orderRepository = orderRepository;
			_orderItemRepository = orderItemRepository;
			_jobItemRepository = jobItemRepository;
			_listItemRepository = listItemRepository;
		}

		public OrderItem Create(Guid id, Guid orderId, int quantity, string partNo, string instructions, int deliveryDays, Guid jobItemId, decimal price)
		{
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
				jobItem.Status = _listItemRepository.GetByType(ListItemType.StatusOrdered);
				jobItem.Location = _listItemRepository.GetByType(ListItemType.WorkLocationSubContract);
				_jobItemRepository.Update(jobItem);
				_jobItemRepository.EmitItemHistory(
					CurrentUser, jobItem.Id, 0, 0, "Item on order OR2000", ListItemType.StatusOrdered, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationSubContract);
				orderItem.JobItem = jobItem;
			}
			_orderItemRepository.Create(orderItem);
			return orderItem;
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