using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Orders;

namespace JobSystem.BusinessLogic.Services
{
	public class OrderService : ServiceBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly ISupplierRepository _supplierRepository;
		private readonly IListItemRepository _listItemRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly OrderItemService _orderItemService;
		private readonly ICompanyDetailsRepository _companyDetailsRepository;

		public OrderService(
			IUserContext applicationContext,
			IOrderRepository orderRepository,
			ISupplierRepository supplierRepository,
			IListItemRepository listItemRepository,
			IEntityIdProvider entityIdProvider,
			OrderItemService orderItemService,
			ICompanyDetailsRepository companyDetailsRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_orderRepository = orderRepository;
			_supplierRepository = supplierRepository;
			_listItemRepository = listItemRepository;
			_entityIdProvider = entityIdProvider;
			_orderItemService = orderItemService;
			_companyDetailsRepository = companyDetailsRepository;
		}

		public Order Create(Guid id, Guid supplierId, string instructions, Guid currencyId)
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
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

		public IEnumerable<Order> GetOrders()
		{
			if (!CurrentUser.HasRole(UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _orderRepository.GetOrders();
		}

		public void CreateOrdersFromPendingItems()
		{
			DoCreateQuotesFromPendingItems(_orderItemService.GetPendingOrderItems());
		}

		public void CreateOrdersFromPendingItems(IList<Guid> pendingItemIds)
		{
			DoCreateQuotesFromPendingItems(_orderItemService.GetPendingOrderItems(pendingItemIds));
		}

		private void DoCreateQuotesFromPendingItems(IEnumerable<PendingOrderItem> pendingItems)
		{
			var defaultCurrencyId = _companyDetailsRepository.GetCompany().DefaultCurrency.Id;
			var supplierGroups = pendingItems.GroupBy(p => p.Supplier.Id);
			foreach (var group in supplierGroups)
			{
				var i = 0;
				var orderId = Guid.NewGuid();
				foreach (var item in group)
				{
					if (i++ == 0)
						Create(orderId, item.Supplier.Id, item.Instructions, defaultCurrencyId);
					_orderItemService.Create(Guid.NewGuid(), orderId, item.Quantity, item.PartNo, item.Instructions, item.DeliveryDays, item.JobItem.Id, item.Price);
					_orderItemService.DeletePendingItem(item.Id);
				}
			}
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