using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.Mvc.ViewModels.Invoices;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.BusinessLogic.Validation.Core;

namespace JobSystem.Mvc.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceItemService _invoiceItemService;
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceItemService invoiceItemService, InvoiceService invoiceService)
        {
            _invoiceItemService = invoiceItemService;
            _invoiceService = invoiceService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ApprovedInvoices");
        }

        [HttpPost]
        [Transaction]
        public ActionResult CreatePending(Guid jobItemId)
        {
            try
            {
                _invoiceItemService.CreatePending(Guid.NewGuid(), jobItemId);
                return Json(new { Success = true, Message = "Successfully Marked for Invoicing" });
            }
            catch (DomainValidationException dex)
            {
                return Json(new { Success = false, Message = dex.Result.ToString() });
            }
        }

        [HttpPost]
        [Transaction]
        public ActionResult ConvertPending(Guid[] ToBeConvertedIds)
        {
            try
            {
                IList<Guid> idList = new List<Guid>();
                if (ToBeConvertedIds.Length > 0)
                    for (var i = 0; i < ToBeConvertedIds.Length; i++)
                        idList.Add(ToBeConvertedIds[i]);
                if (idList.Any())
                    _invoiceService.CreateInvoicesFromPendingItems(idList);
                return RedirectToAction("PendingInvoices");
            }
            catch (DomainValidationException dex)
            {
                return Json(dex.Result);
            }
        }

        public ActionResult PendingInvoices()
        {
            var items = _invoiceItemService.GetPendingInvoiceItems().Select(i => new InvoiceItemIndexViewModel
            {
                Id = i.Id,
                JobItemRef = i.JobItem.GetJobItemRef(),
                CalibrationPrice = (double)i.CalibrationPrice,
                CarriagePrice = (double)i.CarriagePrice,
                Description = i.Description,
                InvestigationPrice = (double)i.InvestigationPrice,
                OrderNo = i.OrderNo,
                PartsPrice = (double)i.PartsPrice,
                RepairPrice = (double)i.RepairPrice
            }).OrderBy(i => i.JobItemRef);
            return View(new InvoiceItemListViewModel
            {
                Items = items,
                Total = _invoiceItemService.GetPendingInvoiceItemsCount()
            });
        }

        public ActionResult ApprovedInvoices(int page = 1)
        {
            var pageSize = 15;
            var items = _invoiceService.GetInvoices().Select(i => new InvoiceIndexViewModel
            {
                Id = i.Id,
                DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
                Currency = i.Currency.Name,
                CustomerName = i.Customer.Name,
                InvoiceNo = i.InvoiceNumber,
                PaymentTerm = i.PaymentTerm.Name,
                TaxCode = i.TaxCode.TaxCodeName,
                ItemCount = _invoiceItemService.GetInvoiceItemsCount(i.Id)
            }).OrderBy(i => i.InvoiceNo).Skip((page - 1) * pageSize).Take(pageSize);
            return View(new InvoiceListViewModel
            {
                Invoices = items,
                Page = page,
                PageSize = pageSize,
                Total = _invoiceService.GetInvoicesCount()
            });
        }

        public ActionResult GenerateInvoiceNote(Guid id)
        {
            return View("RepInvoiceNote", id);
        }
    }
}
