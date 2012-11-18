using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Orders;

namespace JobSystem.Mvc.Controllers
{
	public class OrderController : Controller
	{
		private readonly CompanyDetailsService _companyDetailsService;
		private readonly OrderService _orderService;
		private readonly OrderItemService _orderItemService;
		private readonly CurrencyService _currencyService;
		private readonly ListItemService _listItemService;

		public OrderController(CompanyDetailsService companyDetailsService, OrderService orderService, OrderItemService orderItemService, CurrencyService currencyService, ListItemService listItemService)
		{
			_companyDetailsService = companyDetailsService;
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
		public ActionResult PendingOrderItems(int page = 1)
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
		public ActionResult PendingOrders(int page = 1)
		{
			var items = _orderService.GetOrders().Where(i => !i.IsApproved).Select(
				q => new OrderIndexViewModel
				{
					Id = q.Id,
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name,
					OrderNo = q.OrderNo
				}).OrderBy(o => o.OrderNo).ToList();
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
		public ActionResult ApprovedOrders(int page = 1)
		{
			var items = _orderService.GetOrders().Where(i => i.IsApproved).Select(
				q => new OrderIndexViewModel
				{
					Id = q.Id,
					Instructions = q.Instructions,
					SupplierName = q.Supplier.Name,
					OrderNo = q.OrderNo
				}).OrderBy(o => o.OrderNo).ToList();
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
		public ActionResult ApprovedOrderItems(Guid orderId)
		{
			var orderItemViewModels = new List<OrderItemIndexViewModel>();
			foreach (var item in _orderItemService.GetOrderItems(orderId))
			{
				var dateReceived = new DateTime();
				if (item.DateReceived != null)
					dateReceived = (DateTime)item.DateReceived;
				var viewModel = new OrderItemIndexViewModel
				{
					Id = item.Id,
					DeliveryDays = item.DeliveryDays.ToString(),
					Description = item.Description,
					Instructions = item.Instructions,
					PartNo = item.PartNo,
					Price = item.Price.ToString(),
					Quantity = item.Quantity.ToString(),
					DateReceived = dateReceived.ToLongDateString(),
					IsMarkedReceived = (item.DateReceived != null) ? true : false,
					OrderId = orderId
				};
				if (item.JobItem != null)
					viewModel.JobItemRef = String.Format("{0}/{1}", item.JobItem.Job.JobNo, item.JobItem.ItemNo);
				orderItemViewModels.Add(viewModel);
			}
			return View(orderItemViewModels.OrderBy(o => o.ItemNo).ToList());
		}

		[HttpGet]
		public ActionResult CreateIndividualOrder()
		{
			var viewModel = new OrderCreateViewModel()
			{
				JobItemId = Guid.Empty,
				CurrencyId = _companyDetailsService.GetCompany().DefaultCurrency.Id,
				Currencies = _currencyService.GetCurrencies().ToSelectList()
			};
			return View(viewModel);
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

		public ActionResult Edit(Guid id)
		{
			var order = _orderService.GetById(id);
			return View(new OrderEditViewModel
			{
				Id = order.Id,
				SupplierId = order.Supplier.Id,
				SupplierName = order.Supplier.Name,
				Instructions = order.Instructions,
				CurrencyId = order.Currency.Id,
				Currencies = _currencyService.GetCurrencies().ToSelectList()
			});
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(OrderEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				_orderService.Edit(model.Id, model.SupplierId, model.CurrencyId, model.Instructions);
				return RedirectToAction("Index");
			}
			return View(model);
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
			var viewModel = new OrderItemCreateViewModel { OrderId = orderId };
			return View(viewModel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult CreateItem(OrderItemCreateViewModel viewModel)
		{
			_orderItemService.Create(
				Guid.NewGuid(),
				viewModel.OrderId,
				viewModel.Description,
				viewModel.Quantity,
				viewModel.PartNo,
				viewModel.Instructions,
				viewModel.DeliveryDays,
				Guid.Empty,
				viewModel.Price);
			return RedirectToAction("Details", new { id = viewModel.OrderId });
		}

		[HttpGet]
		public ActionResult Details(Guid id)
		{
			var order = _orderService.GetById(id);
			var viewmodel = new OrderDetailsViewModel
			{
				Id = order.Id,
				Instructions = order.Instructions,
				OrderNo = order.OrderNo,
				SupplierName = order.Supplier.Name,
				DateCreated = order.DateCreated.ToLongDateString() + ' ' + order.DateCreated.ToShortTimeString(),
				CreatedBy = order.CreatedBy.EmailAddress,
				OrderItems = order.OrderItems.Select(o => new OrderItemIndexViewModel
				{
					Id = o.Id,
					ItemNo = o.ItemNo,
					DeliveryDays = o.DeliveryDays.ToString(),
					Description = o.Description,
					Instructions = o.Instructions,
					PartNo = o.PartNo,
					Price = o.Price.ToString(),
					Quantity = o.Quantity.ToString()
				}).OrderBy(o => o.ItemNo).ToList()
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
			var viewModel = new OrderItemEditViewModel()
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
			return View("EditItem", viewModel);
		}

		[HttpGet]
		public ActionResult EditPendingItem(Guid itemId)
		{
			var item = _orderItemService.GetById(itemId);
			var viewModel = new OrderItemEditViewModel
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
				Quantity = item.Quantity
			};
			if (item.JobItem != null)
			{
				viewModel.JobItemId = item.JobItem.Id;
				viewModel.JobId = item.JobItem.Job.Id;
			}
			return View("EditPendingItem", viewModel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditItem(OrderItemEditViewModel viewModel)
		{
			_orderItemService.EditPending(viewModel.Id,
				viewModel.SupplierId,
				viewModel.Description,
				viewModel.Quantity,
				viewModel.PartNo,
				viewModel.Instructions,
				viewModel.DeliveryDays,
				viewModel.Price);
			return RedirectToAction("Details", "Job", new { Id = viewModel.JobId, tabNo = "0" });
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditPendingItem(OrderItemEditViewModel viewModel)
		{
			_orderItemService.Edit(
				viewModel.Id,
				viewModel.Description,
				viewModel.Quantity,
				viewModel.PartNo,
				viewModel.Instructions,
				viewModel.DeliveryDays,
				viewModel.Price);
			return RedirectToAction("Details", "Order", new { id = viewModel.OrderId, tabNo = "0" });
		}

		[HttpPost]
		public ActionResult OrderPending(Guid[] toBeConvertedIds)
		{
			if (ModelState.IsValid)
			{
				if (toBeConvertedIds.Length > 0)
				{
					var transaction = NHibernateSession.Current.BeginTransaction();
					try
					{
						_orderService.CreateOrdersFromPendingItems(toBeConvertedIds);
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

		[Transaction]
		public ActionResult MarkOrderReceived(Guid itemId, Guid orderId)
		{
			_orderItemService.MarkReceived(itemId);
			return RedirectToAction("ApprovedOrderItems", new { orderId = orderId });
		}

		public ActionResult GenerateOrderReport(Guid id)
		{
			return View("RepOrderNote", id);
		}
	}
}