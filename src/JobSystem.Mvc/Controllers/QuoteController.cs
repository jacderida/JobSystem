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
using JobSystem.DataAccess.NHibernate;

namespace JobSystem.Mvc.Controllers
{
    public class QuoteController : Controller
    {
        private readonly QuoteService _quoteService;
        private readonly QuoteItemService _quoteItemService;
        private readonly JobService _jobService;
        private readonly JobItemService _jobItemService;
        private readonly ListItemService _listItemService;
        private readonly CompanyDetailsService _companyDetailsService;
        private readonly CurrencyService _currencyService;

        public QuoteController(
            QuoteService quoteService,
            QuoteItemService quoteItemService,
            JobService jobService,
            JobItemService jobItemService,
            ListItemService listItemService,
            CompanyDetailsService companyDetailsService,
            CurrencyService currencyService)
        {
            _quoteService = quoteService;
            _quoteItemService = quoteItemService;
            _jobService = jobService;
            _jobItemService = jobItemService;
            _listItemService = listItemService;
            _companyDetailsService = companyDetailsService;
            _currencyService = currencyService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ApprovedQuotes");
        }

        [HttpGet]
        public ActionResult Create(Guid jobItemId, Guid jobId)
        {
            var job = _jobService.GetJob(jobId);
            var company = _companyDetailsService.GetCompany();
            var jobItem = _jobItemService.GetById(jobItemId);
            var workTypeListItemId = _listItemService.GetByType(ListItemType.WorkTypeInvestigation).Id;
            var investigationWorkItem = jobItem.HistoryItems.Where(i => i.WorkType.Id == workTypeListItemId).FirstOrDefault();
            var viewmodel = new QuoteCreateViewModel()
            {
                JobItemId = jobItemId,
                JobId = jobId,
                OrderNo = job.OrderNo,
                AdviceNo = job.AdviceNo,
                Currencies = _currencyService.GetCurrencies().ToSelectList(),
                CurrencyId = company.DefaultCurrency.Id,
                Report = investigationWorkItem != null ? investigationWorkItem.Report : String.Empty
            };
            return View("Create", viewmodel);
        }

        [HttpPost]
        public ActionResult Create(QuoteCreateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var transaction = NHibernateSession.Current.BeginTransaction();
                try
                {
                    var job = _jobService.GetJob(viewmodel.JobId);
                    if (viewmodel.IsIndividual)
                    {
                        var quote = _quoteService.Create(Guid.NewGuid(), job.Customer.Id, viewmodel.OrderNo, viewmodel.AdviceNo, viewmodel.CurrencyId);
                        _quoteItemService.Create(
                            Guid.NewGuid(), quote.Id, viewmodel.JobItemId,
                            viewmodel.Repair, viewmodel.Calibration, viewmodel.Parts,
                            viewmodel.Carriage, viewmodel.Investigation, viewmodel.Report,
                            viewmodel.Days, viewmodel.ItemBER);
                    }
                    else
                    {
                        _quoteItemService.CreatePending(
                            Guid.NewGuid(), job.Customer.Id, viewmodel.JobItemId,
                            viewmodel.Repair, viewmodel.Calibration, viewmodel.Parts,
                            viewmodel.Carriage, viewmodel.Investigation, viewmodel.Report,
                            viewmodel.Days, viewmodel.ItemBER, viewmodel.OrderNo, viewmodel.AdviceNo);
                    }
                    transaction.Commit();
                    return RedirectToAction("Details", "Job", new { id = viewmodel.JobId, TabNo = "#tab_3" });
                }
                catch (DomainValidationException dex)
                {
                    transaction.Rollback();
                    ModelState.UpdateFromDomain(dex.Result);
                }
                finally
                {
                    transaction.Dispose();
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
                    Repair = (double)q.Labour,
                    Calibration = (double)q.Calibration,
                    Parts = (double)q.Parts,
                    Carriage = (double)q.Carriage,
                    Investigation = (double)q.Investigation,
                    Days = q.Days,
                    ItemBER = q.BeyondEconomicRepair,
                    JobItemRef = String.Format("{0}/{1}", q.JobItem.Job.JobNo, q.JobItem.ItemNo),
                }).OrderBy(qi => qi.JobItemRef);
            var viewmodel = new QuotePendingListViewModel
            {
                QuoteItems = items,
                Total = _quoteItemService.GetPendingQuoteItemsCount()
            };
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult ApprovedQuotes(int page = 1)
        {
            var pageSize = 15;
            var quotes = _quoteService.GetQuotes().Select(
                q => new QuoteIndexViewModel
                {
                    Id = q.Id,
                    CustomerName = q.Customer.Name,
                    QuoteNo = q.QuoteNumber,
                    CreatedBy = q.CreatedBy.Name,
                    DateCreated = q.DateCreated.ToLongDateString(),
                    OrderNo = q.OrderNumber,
                    AdviceNo = q.AdviceNumber,
                    CurrencyName = q.Currency.Name,
                    ItemCount = _quoteItemService.GetQuoteItemsCount(q.Id)
                }).OrderBy(q => q.QuoteNo).Skip((page - 1) * pageSize).Take(pageSize);
            var viewModel = new QuoteListViewModel
            {
                Items = quotes,
                Page = page,
                PageSize = pageSize,
                Total = _quoteService.GetQuotesCount()
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult QuoteItems(Guid quoteId)
        {
            var quotes = _quoteService.GetById(quoteId);
            var quoteItemsViewModels = new List<QuoteItemIndexViewModel>();
            foreach (var qi in _quoteService.GetById(quoteId).QuoteItems)
            {
                quoteItemsViewModels.Add(new QuoteItemIndexViewModel
                {
                    Id = qi.Id,
                    JobItemId = qi.JobItem.Id,
                    Report = qi.Report,
                    Repair = (double)qi.Labour,
                    Calibration = (double)qi.Calibration,
                    Parts = (double)qi.Parts,
                    Carriage = (double)qi.Carriage,
                    Investigation = (double)qi.Investigation,
                    Days = qi.Days,
                    ItemBER = qi.BeyondEconomicRepair,
                    ItemNo = qi.ItemNo.ToString(),
                    JobItemRef = String.Format("{0}/{1}", qi.JobItem.Job.JobNo, qi.JobItem.ItemNo),
                    Status = qi.Status.Name,
                    StatusType = qi.Status.Type
                });
            }
            return View(quoteItemsViewModels.OrderBy(q => q.JobItemRef).ToList());
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var quote = _quoteService.GetById(id);
            var viewmodel = new QuoteEditViewModel()
            {
                Id = quote.Id,
                AdviceNo = quote.AdviceNumber,
                OrderNo = quote.OrderNumber,
                Currencies = _currencyService.GetCurrencies().ToSelectList(),
                CurrencyId = quote.Currency.Id
            };
            return View("Edit", viewmodel);
        }

        [HttpPost]
        [Transaction]
        public ActionResult Edit(QuoteEditViewModel viewmodel)
        {
            _quoteService.Edit(viewmodel.Id, viewmodel.OrderNo, viewmodel.AdviceNo, viewmodel.CurrencyId);
            return RedirectToAction("ApprovedQuotes");
        }

        [HttpGet]
        public ActionResult EditItem(Guid id, bool fromJi, bool isQuoted)
        {
            if (!isQuoted) 
            {
                var pendingItem = _quoteItemService.GetPendingQuoteItemForJobItem(id);
                var viewModel = new QuoteItemEditViewModel
                {
                    QuoteItemId = pendingItem.Id,
                    JobId = pendingItem.JobItem.Job.Id,
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
                    EditedFromJobItem = fromJi,
                    IsQuoted = isQuoted
                };
                return View("EditItem", viewModel);
            }
            else
            {
                var item = _quoteItemService.GetQuoteItemsForJobItem(id).OrderByDescending(qi => qi.Quote.DateCreated).First();
                var viewModel = new QuoteItemEditViewModel
                {
                    QuoteItemId = item.Id,
                    QuoteId = item.Quote.Id,
                    JobId = item.JobItem.Job.Id,
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
                    EditedFromJobItem = fromJi,
                    IsQuoted = isQuoted
                };
                return View("EditItem", viewModel);
            }
        }

        [HttpPost]
        [Transaction]
        public ActionResult EditItem(QuoteItemEditViewModel viewmodel)
        {
            if (!viewmodel.IsQuoted) 
            {
                _quoteItemService.EditPending(
                    viewmodel.QuoteItemId,
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
            }
            else
            {
                _quoteItemService.Edit(
                    viewmodel.QuoteItemId,
                    viewmodel.Repair,
                    viewmodel.Calibration,
                    viewmodel.Parts,
                    viewmodel.Carriage,
                    viewmodel.Investigation,
                    viewmodel.Report,
                    viewmodel.Days,
                    viewmodel.ItemBER);
            }
            
            if (viewmodel.EditedFromJobItem)
                return RedirectToAction("Details", "Job", new { id = viewmodel.JobId });
            else
            {
                if (!viewmodel.IsQuoted)
                    return RedirectToAction("PendingQuotes");
                else
                    return RedirectToAction("QuoteItems", new { quoteId = viewmodel.QuoteId });
            }
            
        }

        public ActionResult QuotePending(Guid[] toBeConvertedIds)
        {
            if (ModelState.IsValid)
            {
                if (toBeConvertedIds.Length > 0)
                {
                    var transaction = NHibernateSession.Current.BeginTransaction();
                    try
                    {
                        _quoteService.CreateQuotesFromPendingItems(toBeConvertedIds);
                        transaction.Commit();
                    }
                    catch (DomainValidationException dex)
                    {
                        transaction.Rollback();
                        ModelState.UpdateFromDomain(dex.Result);
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }
            return RedirectToAction("PendingQuotes");
        }

        [HttpPost]
        [Transaction]
        public ActionResult AcceptQuoteItem(Guid quoteItemId)
        {
            _quoteItemService.Accept(quoteItemId);
            return Json(true);
        }

        [HttpPost]
        [Transaction]
        public ActionResult RejectQuoteItem(Guid quoteItemId)
        {
            _quoteItemService.Reject(quoteItemId);
            return Json(true);
        }

        [Transaction]
        public ActionResult GenerateQuoteReport(Guid id)
        {
            var quote = _quoteService.GetById(id);
            foreach (var item in quote.QuoteItems)
                _quoteItemService.SetQuoted(item.Id);
            return View("RepQuoteNote", id);
        }
    }
}
