using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.JobItems;
using JobSystem.DataModel.Entities;

namespace JobSystem.Mvc.Controllers
{
    public class JobItemController : Controller
    {
		private readonly JobItemService _jobItemService;
		private readonly ListItemService _listItemService;
		private readonly InstrumentService _instrumentService;

		public JobItemController(JobItemService jobItemService, ListItemService listItemService, InstrumentService instrumentService)
		{
			_jobItemService = jobItemService;
			_listItemService = listItemService;
			_instrumentService = instrumentService;
		}

		[HttpGet]
		public ActionResult Create()
		{
			var viewmodel = new JobItemViewModel()
			{
				Fields = _listItemService.GetAllByType(ListItemType.JobItemField).ToSelectList(),
				Instruments = _instrumentService.GetInstruments().ToSelectList(),
				Locations =  _listItemService.GetAllByType(ListItemType.JobItemLocation).ToSelectList(),
				Status = _listItemService.GetAllByType(ListItemType.JobItemInitialStatus).ToSelectList()
			};

			return PartialView("_Create", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(JobItemViewModel viewmodel)
        {
			if (ModelState.IsValid)
			{
				try
				{
					var id = new System.Guid();
					_jobItemService.CreateJobItem(
						viewmodel.JobId,
						id,
						viewmodel.InstrumentId,
						viewmodel.SerialNo,
						viewmodel.AssetNo,
						viewmodel.InitialStatusId,
						viewmodel.LocationId,
						viewmodel.FieldId,
						viewmodel.CalPeriod,
						viewmodel.Instructions,
						viewmodel.Accessories,
						viewmodel.IsReturned,
						viewmodel.ReturnReason,
						viewmodel.Comments);
					return RedirectToAction("Details", "Job", new { id = viewmodel.JobId });
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewmodel);
        }

    }
}
