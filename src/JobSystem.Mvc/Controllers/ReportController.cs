using System;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Reports;

namespace JobSystem.Mvc.Controllers
{
	public class ReportController : Controller
	{
		private readonly ListItemService _listItemService;
		
		public ReportController(ListItemService listItemService)
		{
			_listItemService = listItemService;
		}

		public ActionResult Index()
		{
			var viewmodel = new JobItemReportViewModel(){
				Status = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemStatus).ToSelectList()
			};
			return View(viewmodel);
		}

		public ActionResult GenerateEquipmentProgressReport(Guid id)
		{
			return View("RepEquipmentProgress", id);
		}

		public ActionResult GenerateJobItemReport(Guid statusId)
		{
			return View();
		}
	}
}