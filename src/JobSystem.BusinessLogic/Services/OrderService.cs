using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class OrderService : ServiceBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly ISupplierRepository _supplierRepository;
		private readonly IListItemRepository _listItemRepository;
		private readonly IEntityIdProvider _entityIdProvider;

		public OrderService(
			IUserContext applicationContext,
			IOrderRepository orderRepository,
			ISupplierRepository supplierRepository,
			IListItemRepository listItemRepository,
			IEntityIdProvider entityIdProvider,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_orderRepository = orderRepository;
			_supplierRepository = supplierRepository;
			_listItemRepository = listItemRepository;
			_entityIdProvider = entityIdProvider;
		}

		public Order Create(Guid id, Guid supplierId, string instructions, Guid currencyId)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the order");
			var order = new Order();
			order.Id = id;
			order.OrderNo = _entityIdProvider.GetIdFor<Order>();
			order.DateCreated = AppDateTime.GetUtcNow();
			order.CreatedBy = CurrentUser;
			order.Supplier = GetSupplier(supplierId);
			order.Instructions = instructions;
			order.Currency = GetCurrency(currencyId);
			ValidateAnnotatedObjectThrowOnFailure(order);
			_orderRepository.Create(order);
			return order;
		}

		private Supplier GetSupplier(Guid supplierId)
		{
			var supplier = _supplierRepository.GetById(supplierId);
			if (supplier == null)
				throw new ArgumentException("A valid ID must be supplied for the supplier");
			return supplier;
		}

		private ListItem GetCurrency(Guid currencyId)
		{
			var currency = _listItemRepository.GetById(currencyId);
			if (currency == null)
				throw new ArgumentException("A valid currency ID must be supplied for the order");
			if (currency.Category.Type != ListItemCategoryType.Currency)
				throw new ArgumentException("A currency list item must be selected for the currency");
			return currency;
		}
	}
}