using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.Mvc.ViewModels.Admin;

namespace JobSystem.Mvc.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			var model = new CompanyDetailsViewModel();
			return View("CompanyDetails", model);
		}

		[Authorize]
		public ActionResult About()
		{
			return View();
		}
	}
}