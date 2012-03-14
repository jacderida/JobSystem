using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Orders;

namespace JobSystem.Mvc.Controllers
{
    public class OrderController : Controller
    {
		private readonly OrderService _orderService;
		private readonly OrderItemService _orderItemService;
		private readonly ListItemService _listItemService;

		public OrderController(OrderService orderService, OrderItemService orderItemService, ListItemService listItemService)
		{
			_orderService = orderService;
			_orderItemService = orderItemService;
			_listItemService = listItemService;
		}

		public ActionResult Index()
		{
			return RedirectToAction("PendingOrders");
		}

		[HttpGet]
		public ActionResult PendingOrders()
		{
			var items = _orderItemService.GetPendingOrderItems().Select(
				q => new OrderIndexViewModel
				{
					Id = q.Id,
					JobItemId = q.JobItem.Id,
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name
				}).ToList();
			return View(items);
		}

		[HttpGet]
		public ActionResult ApprovedOrders()
		{
			var items = _orderService.GetOrders().Select(
				q => new OrderIndexViewModel
				{
					Id = q.Id,
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name,
					OrderNo = q.OrderNo
				}).ToList();
			return View(items);
		}

		[HttpGet]
		public ActionResult CreateIndividualOrder()
		{
			var viewmodel = new OrderCreateViewModel()
			{
				JobItemId = Guid.Empty,
				Currencies = _listItemService.GetAllByCategory(ListItemCategoryType.Currency).ToSelectList()
			};
			return View(viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult CreateIndividualOrder(OrderCreateViewModel viewmodel)
		{
			var order = _orderService.Create(
				Guid.NewGuid(),
				viewmodel.SupplierId,
				viewmodel.Instructions,
				viewmodel.CurrencyId);

			return RedirectToAction("ApprovedOrders");
		}

		[HttpGet]
		public ActionResult Create(Guid jobItemId)
		{
			var viewmodel = new OrderCreateViewModel()
			{ 
				JobItemId = jobItemId,
				Currencies = _listItemService.GetAllByCategory(ListItemCategoryType.Currency).ToSelectList()
			};
			return View(viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(OrderCreateViewModel viewmodel)
		{
			if (viewmodel.IsIndividual)
			{
				var order =_orderService.Create(
					Guid.NewGuid(),
					viewmodel.SupplierId,
					viewmodel.Instructions,
					viewmodel.CurrencyId);
				_orderItemService.Create(
					Guid.NewGuid(),
					order.Id,
					viewmodel.Description,
					viewmodel.Quantity,
					viewmodel.PartNo,
					viewmodel.Instructions,
					viewmodel.DeliveryDays,
					viewmodel.JobItemId,
					viewmodel.Price);
			}
			else
			{
				_orderItemService.CreatePending(
					Guid.NewGuid(),
					viewmodel.SupplierId,
					viewmodel.Description,
					viewmodel.Quantity,
					viewmodel.PartNo,
					viewmodel.Instructions,
					viewmodel.DeliveryDays,
					viewmodel.JobItemId,
					viewmodel.Price);
			}
			return View();
		}

		[HttpGet]
		public ActionResult CreateItem(Guid orderId)
		{
			var viewmodel = new OrderItemCreateViewModel()
			{
				OrderId = orderId,
				Currencies = _listItemService.GetAllByCategory(ListItemCategoryType.Currency).ToSelectList()
			};
			return View(viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult CreateItem(OrderItemCreateViewModel viewmodel)
		{
			_orderItemService.Create(
				Guid.NewGuid(),
				viewmodel.OrderId,
				viewmodel.Description,
				viewmodel.Quantity,
				viewmodel.PartNo,
				viewmodel.Instructions,
				viewmodel.DeliveryDays,
				Guid.Empty,
				viewmodel.Price);

			return RedirectToAction("Details", new { id = viewmodel.OrderId });
		}

		[HttpGet]
		public ActionResult Details(Guid id)
		{
			var order = _orderService.GetById(id);

			var viewmodel = new OrderDetailsViewModel()
			{
				Id = order.Id,
				Instructions = order.Instructions,
				OrderNo = order.OrderNo,
				SupplierName = order.Supplier.Name,
				DateCreated = order.DateCreated.ToLongDateString() + ' ' + order.DateCreated.ToShortTimeString(),
				CreatedBy = order.CreatedBy.EmailAddress,
				OrderItems = order.Items.Select(o => new OrderItemIndexViewModel() {
					DeliveryDays = o.DeliveryDays.ToString(),
					Description = o.Description,
					Instructions = o.Instructions,
					PartNo = o.PartNo,
					Price = o.Price.ToString(),
					Quantity = o.Quantity.ToString()
				}).ToList()
			};
			return View(viewmodel);
		}
    }
}
