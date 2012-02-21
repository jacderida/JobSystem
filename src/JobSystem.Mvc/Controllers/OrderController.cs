using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.Mvc.ViewModels.Orders;

namespace JobSystem.Mvc.Controllers
{
    public class OrderController : Controller
    {
	   //private readonly OrderService _orderService;
		
	   // public OrderController(OrderService orderService)
	   // {
	   //     _orderService = orderService;
	   // }

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
		public ActionResult Create()
		{
			return View();
		}

    }
}
