using System.Web.Mvc;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.ViewModels.Deliveries;
using System;
using System.Linq;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Services;

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
            return RedirectToAction("PendingDeliveries");
        }

		public ActionResult PendingDeliveries()
		{
			var items = _deliveryItemService.GetPendingDeliveryItems().Select(
				i => new DeliveryItemIndexViewModel
				{
					Id = i.Id,
					Notes = i.Notes
				}).ToList();

			return View(items);
		}

		public ActionResult ApprovedDeliveries()
		{
			var items = _deliveryService.GetDeliveries().Select(
				i => new DeliveryIndexViewModel
				{
					Id = i.Id,
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
			return PartialView("_Create", new DeliveryCreateViewModel(){ JobItemId = id });
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(DeliveryCreateViewModel viewmodel)
		{
			var customerId = _jobItemService.GetById(viewmodel.JobItemId).Job.Customer.Id;

			if (viewmodel.IsIndividual)
			{
				var delivery = _deliveryService.Create(viewmodel.Id, customerId, viewmodel.Fao);
				_deliveryItemService.Create(Guid.NewGuid(), delivery.Id, viewmodel.JobItemId, viewmodel.Notes);
			}
			else
			{
				_deliveryItemService.CreatePending(viewmodel.Id, viewmodel.JobItemId, customerId, viewmodel.Notes);
			}

			return PartialView("_Details", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult ConvertPending(Guid[] ToBeConvertedIds)
		{
			List<Guid> idList = new List<Guid>();
			if (ToBeConvertedIds.Length > 0)
			{
				for (var i = 0; i < ToBeConvertedIds.Length; i++)
				{
					idList.Add(ToBeConvertedIds[i]);
				}
			}
			if (idList.Any()) _deliveryService.CreateDeliveriesFromPendingItems(idList);

			return RedirectToAction("PendingDeliveries");
		}

		public ActionResult GenerateDeliveryNote(Guid id)
		{
			return View("RepDeliveryNote", id);
		}
    }
}
