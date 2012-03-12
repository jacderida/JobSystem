using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.ViewModels.Orders;

namespace JobSystem.Mvc.Controllers
{
    public class OrderController : Controller
    {
		private readonly OrderService _orderService;
		private readonly OrderItemService _orderItemService;

		public OrderController(OrderService orderService, OrderItemService orderItemService)
		{
			_orderService = orderService;
		}

		public ActionResult Index()
		{
			IList<OrderIndexViewModel> viewmodels = new List<OrderIndexViewModel>();

			for (int i = 0; i < 10; i++)
			{
				viewmodels.Add(new OrderIndexViewModel()
				{
					Id = i.ToString(),
					Instructions = "aaa",
					JobItemId = "11",
					SupplierId = "aa",
					SupplierName = "SupplierName" + i
				});
			}
			
			return View("PendingOrders", viewmodels);
		}

		[HttpGet]
		public ActionResult Create(Guid? id)
		{
			return View();
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(OrderCreateViewModel viewmodel)
		{
			if (viewmodel.IsIndividual)
			{
				_orderService.Create(
					Guid.NewGuid(),
					viewmodel.SupplierId,
					viewmodel.Instructions,
					viewmodel.CurrencyId);
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

    }
}
