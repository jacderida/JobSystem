using System;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.WorkItems;

namespace JobSystem.Mvc.Controllers
{
    public class WorkItemController : Controller
    {
		private readonly JobItemService _jobItemService;
		private readonly ListItemService _listItemService;

		public WorkItemController(JobItemService jobItemService, ListItemService listItemService)
		{
			_jobItemService = jobItemService;
			_listItemService = listItemService;
		}

		public ActionResult Create(Guid id)
		{
			var viewmodel = new WorkItemViewModel()
			{
				WorkType = _listItemService.GetAllByType(ListItemType.JobItemWorkType).ToSelectList(),
				WorkLocation = _listItemService.GetAllByType(ListItemType.JobItemLocation).ToSelectList(),
				Status = _listItemService.GetAllByType(ListItemType.JobItemWorkStatus).ToSelectList(),
				JobItemId = id
			};

			return PartialView("_Create", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(WorkItemViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_jobItemService.CreateItemHistory(
						System.Guid.NewGuid(),
						viewmodel.JobItemId,
						viewmodel.WorkTime,
						viewmodel.OverTime,
						viewmodel.Report,
						viewmodel.StatusId,
						viewmodel.WorkTypeId,
						viewmodel.WorkLocationId);
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
