using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.JobItems;
using JobSystem.DataModel.Entities;
using System;
using System.Linq;
using JobSystem.Mvc.ViewModels.WorkItems;

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
		public ActionResult Create(Guid id)
		{
			var viewmodel = new JobItemViewModel()
			{
				Fields = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).OrderBy(li => li.Name).ToSelectList(),
				Instruments = _instrumentService.GetInstruments().ToSelectList(),
				Locations =  _listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialLocation).OrderBy(li => li.Name).ToSelectList(),
				Status = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).OrderBy(li => li.Name).ToSelectList(),
				JobId = id
			};
			return View(viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(JobItemViewModel viewmodel)
        {
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
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

		[HttpGet]
		public ActionResult Details(Guid Id)
		{
			var job = _jobItemService.GetById(Id);
			var viewmodel = new JobItemDetailsViewModel()
			{
				Accessories = job.Accessories,
				AssetNo = job.AssetNo,
				CalPeriod = job.CalPeriod,
				Field = job.Field.Name.ToString(),
				InitialStatus = job.InitialStatus.Name.ToString(),
				SerialNo = job.SerialNo,
				Location = job.Location.Name.ToString(),
				Comments = job.Comments,
				Instructions = job.Instructions,
				IsReturned = job.IsReturned,
				ReturnReason = job.ReturnReason,
				WorkItems = job.HistoryItems.Select(wi => new WorkItemDetailsViewModel
				{
					Id = wi.Id,
					JobItemId = wi.JobItem.Id,
					OverTime = wi.OverTime,
					Report = wi.Report,
					Status = wi.Status.Name.ToString(),
					WorkLocation = wi.WorkLocation.Name.ToString(),
					WorkTime = wi.WorkTime,
					WorkType = wi.WorkType.Name.ToString(),
					WorkBy = wi.User.Name,
					DateCreated = wi.DateCreated.ToLongDateString() + ' ' + wi.DateCreated.ToShortTimeString()
					}).ToList()
			};
			viewmodel.InstrumentDetails = String.Format("{0} - {1} : {2}", job.Instrument.ModelNo, job.Instrument.Manufacturer.ToString(), job.Instrument.Description);
			return PartialView("_Details", viewmodel);
		}

    }
}
