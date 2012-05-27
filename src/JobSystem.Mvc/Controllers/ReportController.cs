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
		private readonly CustomerService _customerService;
		
		public ReportController(ListItemService listItemService, CustomerService customerService)
		{
			_listItemService = listItemService;
			_customerService = customerService;
		}

		public ActionResult Index()
		{
			var viewmodel = new JobItemReportViewModel
			{
				Status = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemStatus).ToSelectList(),
				Customer = _customerService.GetCustomers().ToSelectList()
			};
			return View(viewmodel);
		}

		public ActionResult GenerateEquipmentProgressReport(Guid customerId)
		{
			return View("RepEquipmentProgress", customerId);
		}

		public ActionResult GenerateJobItemReport(Guid statusId)
		{
			return View("RepJobItemsAtStatus", statusId);
		}
	}
}