using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Quotes;

namespace JobSystem.Mvc.Controllers
{
	public class QuoteController : Controller
	{
		private readonly QuoteService _quoteService;
		private readonly QuoteItemService _quoteItemService;
		private readonly JobService _jobService;
		private readonly ListItemService _listItemService;
		private readonly CompanyDetailsService _companyDetailsService;

		public QuoteController(QuoteService quoteService, QuoteItemService quoteItemService, JobService jobService, ListItemService listItemService, CompanyDetailsService companyDetailsService)
		{
			_quoteService = quoteService;
			_quoteItemService = quoteItemService;
			_jobService = jobService;
			_listItemService = listItemService;
			_companyDetailsService = companyDetailsService;
		}

		public ActionResult Index()
		{
			return RedirectToAction("PendingQuotes");
		}

		[HttpGet]
		public ActionResult Create(Guid jobItemId, Guid jobId)
		{
			var job = _jobService.GetJob(jobId);
			var company = _companyDetailsService.GetCompany();

			var viewmodel = new QuoteCreateViewModel(){
				JobItemId = jobItemId,
				JobId = jobId,
				OrderNo = job.OrderNo,
				AdviceNo = job.AdviceNo,
				Currencies = _listItemService.GetAllByCategory(ListItemCategoryType.Currency).ToSelectList(),
				CurrencyId = company.DefaultCurrency.Id
			};
			return View("Create", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(QuoteCreateViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var job = _jobService.GetJob(viewmodel.JobId);

					if (viewmodel.IsIndividual)
					{
						var quote = _quoteService.Create(
							Guid.NewGuid(),
							job.Customer.Id,
							viewmodel.OrderNo,
							viewmodel.AdviceNo,
							viewmodel.CurrencyId
						);

						_quoteItemService.Create(
							Guid.NewGuid(),
							quote.Id,
							viewmodel.JobItemId,
							viewmodel.Repair,
							viewmodel.Calibration,
							viewmodel.Parts,
							viewmodel.Carriage,
							viewmodel.Investigation,
							viewmodel.Report,
							viewmodel.Days,
							viewmodel.ItemBER
						);
					}
					else
					{
						_quoteItemService.CreatePending(
							Guid.NewGuid(),
							job.Customer.Id,
							viewmodel.JobItemId,
							viewmodel.Repair,
							viewmodel.Calibration,
							viewmodel.Parts,
							viewmodel.Carriage,
							viewmodel.Investigation,
							viewmodel.Report,
							viewmodel.Days,
							viewmodel.ItemBER,
							viewmodel.OrderNo,
							viewmodel.AdviceNo
						);
					}
					return RedirectToAction("Details", "Job", new { id = viewmodel.JobId, TabNo = "#tab_3" });
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View("Create", viewmodel);
		}

		[HttpGet]
		public ActionResult PendingQuotes()
		{
			var items = _quoteItemService.GetPendingQuoteItems().Select(
				q => new QuoteItemIndexViewModel
				{
					Id = q.Id,
					JobItemId = q.JobItem.Id,
					JobNo = q.JobItem.Job.JobNo,
					AdviceNo = q.AdviceNo,
					OrderNo = q.OrderNo,
					Report = q.Report,
					Repair = q.Labour,
					Calibration = q.Calibration,
					Parts = q.Parts,
					Carriage = q.Carriage,
					Investigation = q.Investigation,
					Days = q.Days,
					ItemBER = q.BeyondEconomicRepair
				}).ToList();

			return View(items);
		}

		[HttpGet]
		public ActionResult ApprovedQuotes()
		{
			var quotes = _quoteService.GetQuotes().Select(
			    q => new QuoteIndexViewModel
			    {
			        Id = q.Id,
					CustomerName = q.Customer.Name,
					QuoteNo = q.QuoteNumber,
					CreatedBy = q.CreatedBy.Name,
					DateCreated = q.DateCreated.ToLongDateString() + ' ' + q.DateCreated.ToShortTimeString(),
					OrderNo = q.OrderNumber,
					AdviceNo = q.AdviceNumber
			    }).ToList();

			foreach (var quote in quotes)
			{
				var quoteItems = _quoteItemService.GetQuoteItems(quote.Id);
				quote.QuoteItems = quoteItems.Select(qi => new QuoteItemIndexViewModel
						{
							Report = qi.Report,
							Repair = qi.Labour,
							Calibration = qi.Calibration,
							Parts = qi.Parts,
							Carriage = qi.Carriage,
							Investigation = qi.Investigation,
							Days = qi.Days,
							ItemBER = qi.BeyondEconomicRepair,
							ItemNo = qi.ItemNo.ToString()
							}).ToList();
			}
			return View(quotes);
		}

		[HttpGet]
		public ActionResult EditPending(Guid id)
		{
			var pendingItem = _quoteItemService.GetPendingQuoteItemForJobItem(id);

			var viewmodel = new QuoteItemEditViewModel()
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
				Report = pendingItem.Report
			};
			return View("Edit", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditPending(QuoteItemEditViewModel viewmodel)
		{
			_quoteItemService.EditPending(
				viewmodel.Id,
				viewmodel.Repair,
				viewmodel.Calibration,
				viewmodel.Parts,
				viewmodel.Carriage,
				viewmodel.Investigation,
				viewmodel.Report,
				viewmodel.Days,
				viewmodel.ItemBER,
				viewmodel.OrderNo,
				viewmodel.AdviceNo);

			return RedirectToAction("PendingQuotes");
		}

		[Transaction]
		public ActionResult QuotePending(Guid[] ToBeConvertedIds)
		{
			if (ModelState.IsValid)
			{
				try
				{
					IList<Guid> idList = new List<Guid>();
					if (ToBeConvertedIds.Length > 0)
					{
						for (var i = 0; i < ToBeConvertedIds.Length; i++)
						{
							idList.Add(ToBeConvertedIds[i]);
						}
					}
					//if (idList.Any()) _quoteService.CreateQuotesFromPendingItems(idList);

					return RedirectToAction("PendingConsignments");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return RedirectToAction("PendingConsignments");
		}

		//[HttpGet]
		//public ActionResult EditPendingItem(Guid id)
		//{
		//    var quote = _quoteItemService.GetPendingItem(id);

		//    var viewmodel = new QuoteItemEditViewModel()
		//    {
		//        Id = quote.Id,
		//        AdviceNo = ,
		//        Calibration = ,
		//        Carriage = ,
		//        Days = ,
		//        Investigation = ,
		//        ItemBER = ,
		//        OrderNo = ,
		//        Parts = ,
		//        Repair = ,
		//        Report = 
		//    };
		//    return PartialView("_Edit", viewmodel);
		//}

		//[HttpPost]
		//[Transaction]
		//public ActionResult EditPending(QuoteItemEditViewModel viewmodel)
		//{
		//    _quoteItemService.EditPending(
		//        viewmodel.Id,
		//        viewmodel.JobItemId,
		//        viewmodel.SupplierId,
		//        viewmodel.Instructions);

		//    return RedirectToAction("PendingQuotes", "Quote");
		//}
	}
}
