using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.Admin.Mvc.Data;
using JobSystem.Admin.Mvc.Models;

namespace JobSystem.Admin.Mvc.Controllers
{
	public class TenantController : Controller
	{
		private readonly ITenantRepository _tenantRepository;

		public TenantController(ITenantRepository tenantRepository)
		{
			_tenantRepository = tenantRepository;
		}

		public ActionResult Index()
		{
			return View(_tenantRepository.GetTenants().Select(t => new TenantIndexModel { Id = t.Item1, Name = t.Item2 }));
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
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}