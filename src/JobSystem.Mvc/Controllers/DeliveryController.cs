using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Deliveries;

namespace JobSystem.Mvc.Controllers
{
	public class DeliveryController : Controller
	{
		private readonly DeliveryService _deliveryService;
		private readonly DeliveryItemService _deliveryItemService;
		private readonly JobItemService _jobItemService;

		public DeliveryController(DeliveryService deliveryService, DeliveryItemService deliveryItemService, JobItemService jobItemService)
		{
			_deliveryService = deliveryService;
			_deliveryItemService = deliveryItemService;
			_jobItemService = jobItemService;
		}

		public ActionResult Index()
		{
			return RedirectToAction("ApprovedDeliveries");
		}

		public ActionResult PendingDeliveries(int page = 1)
		{
			var items = _deliveryItemService.GetPendingDeliveryItems().Select(
				di => new DeliveryItemIndexViewModel
				{
					Id = di.Id,
					JobRef = String.Format("{0}/{1}", di.JobItem.Job.JobNo, di.JobItem.ItemNo),
					Customer = di.Customer.Name,
					Notes = di.Notes
				}).OrderBy(di => di.JobRef).ToList();
			return View(items);
		}

		public ActionResult ApprovedDeliveries(int page = 1)
		{
			var items = _deliveryService.GetDeliveries().Select(
				i => new DeliveryIndexViewModel
				{
					Id = i.Id,
					DeliveryNo = i.DeliveryNoteNumber,
					CustomerName = i.Customer.Name,
					CreatedBy = i.CreatedBy.Name,
					DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
					Fao = i.Fao
				}).ToList();
			foreach (var item in items)
			{
				var deliveryItems = _deliveryItemService.GetDeliveryItems(item.Id);
				item.DeliveryItems = deliveryItems.Select(di => new DeliveryItemIndexViewModel
				{
					Notes = di.Notes
				}).ToList();
			}
			return View(items);
		}

		[HttpGet]
		public ActionResult Create(Guid id)
		{
			return PartialView("_Create", new DeliveryCreateViewModel() { JobItemId = id });
		}

		[HttpPost]
		public ActionResult Create(DeliveryCreateViewModel viewmodel)
		{
			var customerId = _jobItemService.GetById(viewmodel.JobItemId).Job.Customer.Id;
			var transaction = NHibernateSession.Current.BeginTransaction();
			try
			{
				if (viewmodel.IsIndividual)
				{
					var delivery = _deliveryService.Create(viewmodel.Id, customerId, viewmodel.Fao);
					_deliveryItemService.Create(Guid.NewGuid(), delivery.Id, viewmodel.JobItemId, viewmodel.Notes);
				}
				else
					_deliveryItemService.CreatePending(viewmodel.Id, viewmodel.JobItemId, customerId, viewmodel.Notes);
				transaction.Commit();
				return RedirectToAction("Details", "JobItem", new { Id = viewmodel.JobItemId });
			}
			catch (DomainValidationException dex)
			{
				if (transaction.IsActive)
					transaction.Rollback();
				ModelState.UpdateFromDomain(dex.Result);
			}
			finally
			{
				transaction.Dispose();
			}
			return PartialView("_Details", viewmodel);
		}

		[HttpPost]
		public ActionResult ConvertPending(Guid[] ToBeConvertedIds)
		{
			if (ToBeConvertedIds.Length > 0)
			{
				var transaction = NHibernateSession.Current.BeginTransaction();
				try
				{
					_deliveryService.CreateDeliveriesFromPendingItems(ToBeConvertedIds.Select(id => id).ToList());
					transaction.Commit();
				}
				catch (DomainValidationException dex)
				{
					transaction.Rollback();
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return RedirectToAction("PendingDeliveries");
		}

		public ActionResult GenerateDeliveryNote(Guid id)
		{
			return View("RepDeliveryNote", id);
		}
	}
}