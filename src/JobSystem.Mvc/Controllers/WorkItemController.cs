using System;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.WorkItems;

namespace JobSystem.Mvc.Controllers
{
	public class WorkItemController : Controller
	{
		private readonly ItemHistoryService _itemHistoryService;
		private readonly ListItemService _listItemService;

		public WorkItemController(ItemHistoryService jobItemService, ListItemService listItemService)
		{
			_itemHistoryService = jobItemService;
			_listItemService = listItemService;
		}

		public ActionResult Create(Guid id)
		{
			var viewmodel = new WorkItemCreateViewModel()
			{
				WorkType = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemWorkType).ToSelectList(),
				WorkLocation = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemLocation).ToSelectList(),
				Status = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemWorkStatus).ToSelectList(),
				JobItemId = id
			};
			return PartialView("_Create", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(WorkItemCreateViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_itemHistoryService.CreateItemHistory(
						System.Guid.NewGuid(),
						viewmodel.JobItemId,
						viewmodel.WorkTime,
						viewmodel.OverTime,
						viewmodel.Report,
						viewmodel.StatusId,
						viewmodel.WorkTypeId,
						viewmodel.WorkLocationId);
					return RedirectToAction("Details", "JobItem", new { Id = viewmodel.JobItemId });
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return PartialView("_Create", viewmodel);
		}
	}
}