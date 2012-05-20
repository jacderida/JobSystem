using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Orders;

namespace JobSystem.Mvc.Controllers
{
	public class OrderController : Controller
	{
		private readonly OrderService _orderService;
		private readonly OrderItemService _orderItemService;
		private readonly CurrencyService _currencyService;
		private readonly ListItemService _listItemService;

		public OrderController(OrderService orderService, OrderItemService orderItemService, CurrencyService currencyService, ListItemService listItemService)
		{
			_orderService = orderService;
			_orderItemService = orderItemService;
			_listItemService = listItemService;
			_currencyService = currencyService;
		}

		public ActionResult Index()
		{
			return RedirectToAction("ApprovedOrders");
		}

		[HttpGet]
		public ActionResult PendingOrderItems()
		{
			var items = _orderItemService.GetPendingOrderItems().Select(
				q => new OrderItemIndexViewModel
				{
					Id = q.Id,
					JobItemId = q.JobItem.Id,
					DeliveryDays = q.DeliveryDays.ToString(),
					Description = q.Description,
					PartNo = q.PartNo,
					Price = q.Price.ToString(),
					Quantity = q.Quantity.ToString(),
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name
				}).ToList();
			return View(items);
		}

		[HttpGet]
		public ActionResult PendingOrders()
		{
			var items = _orderService.GetOrders().Where(i => !i.IsApproved).Select(
				q => new OrderIndexViewModel
				{
					Id = q.Id,
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name,
					OrderNo = q.OrderNo
				}).ToList();

			foreach (var item in items)
			{
				var orderItems = _orderItemService.GetOrderItems(item.Id);
				item.OrderItems = orderItems.Select(oi => new OrderItemIndexViewModel()
				{
					DeliveryDays = oi.DeliveryDays.ToString(),
					Description = oi.Description,
					Instructions = oi.Instructions,
					PartNo = oi.PartNo,
					Price = oi.Price.ToString(),
					Quantity = oi.Quantity.ToString()
				}).ToList();
			}
			return View(items);
		}

		[HttpGet]
		public ActionResult ApprovedOrders()
		{
			var items = _orderService.GetOrders().Where(i => i.IsApproved).Select(
				q => new OrderIndexViewModel
				{
					Id = q.Id,
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name,
					OrderNo = q.OrderNo
				}).ToList();

			foreach (var item in items)
			{
				var orderItems = _orderItemService.GetOrderItems(item.Id);
				item.OrderItems = orderItems.Select(oi => new OrderItemIndexViewModel()
				{
					DeliveryDays = oi.DeliveryDays.ToString(),
					Description = oi.Description,
					Instructions = oi.Instructions,
					PartNo = oi.PartNo,
					Price = oi.Price.ToString(),
					Quantity = oi.Quantity.ToString()
				}).ToList();
			}
			return View(items);
		}

		[HttpGet]
		public ActionResult CreateIndividualOrder()
		{
			var viewmodel = new OrderCreateViewModel()
			{
				JobItemId = Guid.Empty,
				Currencies = _currencyService.GetCurrencies().ToSelectList()
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

			return RedirectToAction("PendingOrders");
		}

		[HttpGet]
		public ActionResult Create(Guid jobItemId, Guid jobId)
		{
			var viewmodel = new OrderCreateViewModel()
			{
				JobId = jobId,
				JobItemId = jobItemId,
				Currencies = _currencyService.GetCurrencies().ToSelectList()
			};
			return View(viewmodel);
		}

		[HttpPost]
		public ActionResult Create(OrderCreateViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				var transaction = NHibernateSession.Current.BeginTransaction();
				try
				{
					if (viewmodel.IsIndividual)
					{
						var order = _orderService.Create(Guid.NewGuid(), viewmodel.SupplierId, viewmodel.Instructions, viewmodel.CurrencyId);
						_orderItemService.Create(
							Guid.NewGuid(), order.Id, viewmodel.Description,
							viewmodel.Quantity, viewmodel.PartNo, viewmodel.Instructions,
							viewmodel.DeliveryDays, viewmodel.JobItemId, viewmodel.Price);
					}
					else
					{
						_orderItemService.CreatePending(
							Guid.NewGuid(), viewmodel.SupplierId, viewmodel.Description,
							viewmodel.Quantity, viewmodel.PartNo, viewmodel.Instructions,
							viewmodel.DeliveryDays, viewmodel.JobItemId, viewmodel.Price);
					}
					transaction.Commit();
					return RedirectToAction("Details", "Job", new { id = viewmodel.JobId, tabNo = "0" });
				}
				catch (DomainValidationException dex)
				{
					transaction.Rollback();
					ModelState.UpdateFromDomain(dex.Result);
				}
				finally
				{
					transaction.Dispose();
				}
			}
			return View("Create", viewmodel);
		}

		[HttpGet]
		public ActionResult CreateItem(Guid orderId)
		{
			var viewmodel = new OrderItemCreateViewModel()
			{
				OrderId = orderId,
				Currencies = _currencyService.GetCurrencies().ToSelectList()
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
				OrderItems = order.OrderItems.Select(o => new OrderItemIndexViewModel()
				{
					Id = o.Id,
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

		[Transaction]
		public ActionResult ApproveOrder(Guid id)
		{
			_orderService.ApproveOrder(id);

			return RedirectToAction("PendingOrders");
		}

		[HttpGet]
		public ActionResult EditItem(Guid jobItemId)
		{
			var item = _orderItemService.GetPendingOrderItemForJobItem(jobItemId);

			var viewmodel = new OrderItemEditViewModel()
			{
				Id = item.Id,
				DeliveryDays = item.DeliveryDays,
				Description = item.Description,
				Instructions = item.Instructions,
				PartNo = item.PartNo,
				Price = item.Price,
				Quantity = item.Quantity,
				SupplierId = item.Supplier.Id,
				SupplierName = item.Supplier.Name,
				JobItemId = item.JobItem.Id,
				JobId = item.JobItem.Job.Id
			};

			return View("EditItem", viewmodel);
		}

		[HttpGet]
		public ActionResult EditPendingItem(Guid itemId)
		{
			var item = _orderItemService.GetById(itemId);

			var viewmodel = new OrderItemEditViewModel()
			{
				Id = item.Id,
				DeliveryDays = item.DeliveryDays,
				Description = item.Description,
				Instructions = item.Instructions,
				SupplierId = item.Order.Supplier.Id,
				SupplierName = item.Order.Supplier.Name,
				PartNo = item.PartNo,
				Price = item.Price,
				OrderId = item.Order.Id,
				Quantity = item.Quantity,
				JobItemId = item.JobItem.Id,
				JobId = item.JobItem.Job.Id
			};

			return View("EditPendingItem", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditItem(OrderItemEditViewModel viewmodel)
		{
			_orderItemService.EditPending(viewmodel.Id,
				viewmodel.SupplierId,
				viewmodel.Description,
				viewmodel.Quantity,
				viewmodel.PartNo,
				viewmodel.Instructions,
				viewmodel.DeliveryDays,
				viewmodel.Price);

			return RedirectToAction("Details", "Job", new { Id = viewmodel.JobId, tabNo = "0" });
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditPendingItem(OrderItemEditViewModel viewmodel)
		{
			_orderItemService.EditPending(viewmodel.Id,
				viewmodel.SupplierId,
				viewmodel.Description,
				viewmodel.Quantity,
				viewmodel.PartNo,
				viewmodel.Instructions,
				viewmodel.DeliveryDays,
				viewmodel.Price);

			return RedirectToAction("Details", "Order", new { id = viewmodel.OrderId, tabNo = "0" });
		}

		[HttpPost]
		public ActionResult OrderPending(Guid[] ToBeConvertedIds)
		{
			if (ModelState.IsValid)
			{
				if (ToBeConvertedIds.Length > 0)
				{
					var transaction = NHibernateSession.Current.BeginTransaction();
					try
					{
						_orderService.CreateOrdersFromPendingItems(ToBeConvertedIds);
						transaction.Commit();
					}
					catch (DomainValidationException dex)
					{
						transaction.Rollback();
						ModelState.UpdateFromDomain(dex.Result);
					}
					finally
					{
						transaction.Dispose();
					}
				}
			}
			return RedirectToAction("PendingOrders");
		}

		public ActionResult GenerateOrderReport(Guid id)
		{
			return View("RepOrderNote", id);
		}
	}
}