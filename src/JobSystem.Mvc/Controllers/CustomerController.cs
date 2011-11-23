using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSystem.Mvc.Controllers
{
	public class CustomerController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Details(Guid id)
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		public ActionResult Edit(int id)
		{
			return View();
		}

		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}