﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSystem.Admin.Mvc.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return RedirectToAction("Index", "Tenant");
		}

		[Authorize]
		public ActionResult About()
		{
			return View();
		}
	}
}