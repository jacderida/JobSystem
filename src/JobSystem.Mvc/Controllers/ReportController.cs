using System;
using System.Web.Mvc;

namespace JobSystem.Mvc.Controllers
{
	public class ReportController : Controller
	{
		public ReportController()
		{

		}

		public ActionResult GenerateEquipmentProgressReport(Guid id)
		{
			return View("RepEquipmentProgress", id);
		}
	}
}