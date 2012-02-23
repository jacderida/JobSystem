using System;
using System.Web.Mvc;
using System.Linq;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.ViewModels.Consignments;
using System.Collections;
using System.Collections.Generic;

namespace JobSystem.Mvc.Controllers
{
    public class ConsignmentController : Controller
    {
        private readonly ConsignmentService _consignmentService;
		private readonly ConsignmentItemService _consignmentItemService;
		
		public ConsignmentController(ConsignmentService consignmentService, ConsignmentItemService consignmentItemService)
		{
			_consignmentService = consignmentService;
			_consignmentItemService = consignmentItemService;
		}

		public ActionResult Index()
		{
			//Placeholder admin role check to see whether user should be shown pending or approved jobs by default
			var isAdmin = true;
			if (isAdmin)
				return RedirectToAction("PendingConsignments");
			return RedirectToAction("ActiveConsignments");
		}

        public ActionResult Create(Guid Id)
        {
            var viewmodel = new ConsignmentCreateViewModel(){
				JobItemId = Id
			};
			return PartialView("_Create", viewmodel);
        }

		[HttpPost]
		[Transaction]
		public ActionResult Create(ConsignmentCreateViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (viewmodel.IsIndividual) 
					{
						var consignment = _consignmentService.Create(System.Guid.NewGuid(), viewmodel.SupplierId);
						_consignmentItemService.Create(
							Guid.NewGuid(),
							viewmodel.JobItemId,
							consignment.Id,
							viewmodel.Instructions
						);
					}
					else 
					{
						_consignmentItemService.CreatePending(
							Guid.NewGuid(),
							viewmodel.JobItemId,
							viewmodel.SupplierId,
							viewmodel.Instructions
						);
					}
					return RedirectToAction("Details", "JobItem", new { Id = viewmodel.JobItemId });
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return PartialView("_Create", viewmodel);
		}

		public ActionResult PendingConsignments()
		{
			var items = _consignmentItemService.GetPendingItems().Select(
				c => new ConsignmentItemIndexViewModel
				{
					Id = c.Id,
					JobItemId = c.JobItem.Id,
					Instructions = c.Instructions,
					SupplierName = c.Supplier.Name
				}).ToList();
			var viewmodel = new ConsignmentPendingListViewModel(){
				ConsignmentItems = items
			};
			return View(viewmodel);
		}

		public ActionResult ActiveConsignments()
		{
			var items = _consignmentService.GetConsignments().Select(
			    c => new ConsignmentIndexViewModel
			    {
			        Id = c.Id,
			        ConsignmentNo = c.ConsignmentNo,
			        CreatedBy = c.CreatedBy.Name,
					DateCreated = c.DateCreated.ToLongDateString() + ' ' + c.DateCreated.ToShortTimeString(),
					SupplierName = c.Supplier.Name
			    }).ToList();

			foreach (var item in items)
			{
				var consignmentItems = _consignmentItemService.GetConsignmentItems(item.Id);
				item.ConsignmentItems = consignmentItems.Select(ci => new ConsignmentItemIndexViewModel
						{
							Instructions = ci.Instructions,
							InstrumentDetails = String.Format("{0} - {1} : {2}", ci.JobItem.Instrument.ModelNo, ci.JobItem.Instrument.Manufacturer.ToString(), ci.JobItem.Instrument.Description),
							}).ToList();
			}
			return View(items);
		}
		
		[Transaction]
		public ActionResult ConsignPending(Guid[] ToBeConvertedIds)
		{
			IList<Guid> idList = new List<Guid>();
			if (ToBeConvertedIds.Length > 0)
			{
				for (var i = 0; i < ToBeConvertedIds.Length; i++)
				{
					idList.Add(ToBeConvertedIds[i]);
				}
			}
			if (idList.Any()) _consignmentService.CreateConsignmentsFromPendingItems(idList);
			
			return RedirectToAction("PendingConsignments");
		}
    }
}
