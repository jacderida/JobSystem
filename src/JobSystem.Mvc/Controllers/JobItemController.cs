using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.JobItems;
using JobSystem.Mvc.ViewModels.WorkItems;
using JobSystem.Mvc.ViewModels.Consignments;
using JobSystem.Mvc.ViewModels.Quotes;

namespace JobSystem.Mvc.Controllers
{
    public class JobItemController : Controller
    {
		private readonly JobItemService _jobItemService;
		private readonly ListItemService _listItemService;
		private readonly InstrumentService _instrumentService;
		private readonly ConsignmentService _consignmentService;
		private readonly QuoteItemService _quoteItemService;

		public JobItemController(JobItemService jobItemService, ListItemService listItemService, InstrumentService instrumentService, ConsignmentService consignmentService, QuoteItemService quoteItemService)
		{
			_jobItemService = jobItemService;
			_listItemService = listItemService;
			_instrumentService = instrumentService;
			_consignmentService = consignmentService;
			_quoteItemService = quoteItemService;
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
				Id = job.Id,
				JobId = job.Job.Id,
				Accessories = job.Accessories,
				AssetNo = job.AssetNo,
				CalPeriod = job.CalPeriod,
				Field = job.Field.Name.ToString(),
				InitialStatus = job.Status.Name.ToString(),
				SerialNo = job.SerialNo,
				Location = job.Location.Name.ToString(),
				Comments = job.Comments,
				Instructions = job.Instructions,
				IsReturned = job.IsReturned,
				ReturnReason = job.ReturnReason,
				QuoteItem = PopulateQuoteItemViewModel(job.Id),
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
				}).OrderByDescending(wi => wi.DateCreated).ToList()
			};
			viewmodel.InstrumentDetails = String.Format("{0} - {1} : {2}", job.Instrument.ModelNo, job.Instrument.Manufacturer.ToString(), job.Instrument.Description);

			var pendingItem = _jobItemService.GetPendingConsignmentItem(Id);
			if (pendingItem == null)
			{
				var item = _jobItemService.GetLatestConsignmentItem(Id);
				if (item != null) 
				{
					if (item.Consignment != null)
					{
						viewmodel.Consignment = new ConsignmentIndexViewModel()
						{
							Id = item.Consignment.Id,
							ConsignmentNo = item.Consignment.ConsignmentNo,
							CreatedBy = item.Consignment.CreatedBy.Name,
							DateCreated = item.Consignment.DateCreated.ToLongDateString() + ' ' + item.Consignment.DateCreated.ToShortTimeString(),
							SupplierName = item.Consignment.Supplier.Name
						};
					}
					else
					{
						viewmodel.ConsignmentItem = new ConsignmentItemIndexViewModel()
						{
							Id = item.Id,
							Instructions = item.Instructions,
							SupplierName = item.Consignment.Supplier.Name
						};
					}
				} 
				else 
				{
					viewmodel.ConsignmentItem = null;
				}
			}
			else
			{
				viewmodel.ConsignmentItem = new ConsignmentItemIndexViewModel()
				{
					Id = pendingItem.Id,
					Instructions = pendingItem.Instructions,
					SupplierName = pendingItem.Supplier.Name
				};
			}
			return PartialView("_Details", viewmodel);
		}

		private QuoteItemIndexViewModel PopulateQuoteItemViewModel(Guid Id)
		{
			var pendingItem = _quoteItemService.GetPendingQuoteItemForJobItem(Id);
			if (pendingItem == null)
			{
				var item = _quoteItemService.GetQuoteItemForJobItem(Id);
				if (item != null)
				{
				    var viewmodel = new QuoteItemIndexViewModel()
				    {
						Id = item.Id,
						JobItemId = item.JobItem.Id,
						AdviceNo = item.Quote.AdviceNumber,
						Calibration = item.Calibration,
						Carriage = item.Carriage,
						Days = item.Days,
						Investigation = item.Investigation,
						ItemBER = item.BeyondEconomicRepair,
						OrderNo = item.Quote.OrderNumber,
						Parts = item.Parts,
						Repair = item.Labour,
						Report = item.Report,
						IsQuoted = true,
						JobItemNo = item.JobItem.ItemNo.ToString()
				    };
				    return viewmodel;
				}
				else
				{
				    return null;
				}
			}
			else
			{
				var viewmodel = new QuoteItemIndexViewModel()
				{
					Id = pendingItem.Id,
					JobItemId = pendingItem.JobItem.Id,
					AdviceNo = pendingItem.AdviceNo,
					Calibration = pendingItem.Calibration,
					Carriage = pendingItem.Carriage,
					Days = pendingItem.Days,
					Investigation = pendingItem.Investigation,
					ItemBER = pendingItem.BeyondEconomicRepair,
					OrderNo = pendingItem.OrderNo,
					Parts = pendingItem.Parts,
					Repair = pendingItem.Labour,
					Report = pendingItem.Report,
					IsQuoted = false,
					JobItemNo = pendingItem.JobItem.ItemNo.ToString()
				};
				return viewmodel;
			}
		}
    }
}
