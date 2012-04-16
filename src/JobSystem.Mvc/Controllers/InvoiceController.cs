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
		//
        // GET: /Invoice/

        public ActionResult Index()
        {
            return RedirectToAction("PendingInvoices");
        }

		[HttpPost]
		[Transaction]
		public ActionResult CreatePending(Guid jobItemId)
		{
			try
			{
				_invoiceItemService.CreatePending(Guid.NewGuid(), jobItemId);
				return Json(true);
			}
			catch (DomainValidationException dex)
			{
				return Json(dex.Result);
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
				{
					for (var i = 0; i < ToBeConvertedIds.Length; i++)
					{
						idList.Add(ToBeConvertedIds[i]);
					}
				}
				if (idList.Any()) _invoiceService.CreateInvoicesFromPendingItems(idList);

				return RedirectToAction("PendingInvoices");
			}
			catch (DomainValidationException dex)
			{
				return Json(dex.Result);
			}
		}

		public ActionResult PendingInvoices()
		{
			var items = _invoiceItemService.GetPendingInvoiceItems().Select(i => new InvoiceItemIndexViewModel{
				Id = i.Id,
				JobNo = i.JobItem.Job.JobNo,
				CalibrationPrice = (double)i.CalibrationPrice,
				CarriagePrice = (double)i.CarriagePrice,
				Description = i.Description,
				InvestigationPrice = (double)i.InvestigationPrice,
				JobItemNo = i.JobItem.ItemNo.ToString(),
				OrderNo = i.OrderNo,
				PartsPrice = (double)i.PartsPrice,
				RepairPrice = (double)i.RepairPrice
			}).ToList();

			return View(items);
		}

		public ActionResult ApprovedInvoices()
		{
			var items = _invoiceService.GetInvoices().Select(i => new InvoiceIndexViewModel
			{
				Id = i.Id,
				DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
				Currency = i.Currency.Name,
				CustomerName = i.Customer.Name,
				InvoiceNo = i.InvoiceNumber,
				PaymentTerm = i.PaymentTerm.Name,
				TaxCode = i.TaxCode.TaxCodeName,
				InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemIndexViewModel{
					Id = item.Id,
					JobNo = item.JobItem.Job.JobNo,
					CalibrationPrice = (double)item.CalibrationPrice,
					CarriagePrice = (double)item.CarriagePrice,
					Description = item.Description,
					InvestigationPrice = (double)item.InvestigationPrice,
					JobItemNo = item.JobItem.ItemNo.ToString(),
					PartsPrice = (double)item.PartsPrice,
					RepairPrice = (double)item.RepairPrice
				}).ToList()
			}).ToList();

			return View(items);
		}
    }
}
