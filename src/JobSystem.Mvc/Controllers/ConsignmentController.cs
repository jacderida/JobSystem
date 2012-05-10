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
using JobSystem.DataAccess.NHibernate;

namespace JobSystem.Mvc.Controllers
{
	public class ConsignmentController : Controller
	{
		private readonly ConsignmentService _consignmentService;
		private readonly ConsignmentItemService _consignmentItemService;
		private readonly OrderService _orderService;

		public ConsignmentController(ConsignmentService consignmentService, ConsignmentItemService consignmentItemService, OrderService orderService)
		{
			_consignmentService = consignmentService;
			_consignmentItemService = consignmentItemService;
			_orderService = orderService;
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
			var viewmodel = new ConsignmentCreateViewModel()
			{
				JobItemId = Id
			};
			return PartialView("_Create", viewmodel);
		}

		[HttpPost]
		public ActionResult Create(ConsignmentCreateViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				var transaction = NHibernateSession.Current.BeginTransaction();
				try
				{
					if (viewmodel.IsIndividual)
					{
						var consignment = _consignmentService.Create(System.Guid.NewGuid(), viewmodel.SupplierId);
						_consignmentItemService.Create(Guid.NewGuid(), viewmodel.JobItemId, consignment.Id, viewmodel.Instructions);
					}
					else
						_consignmentItemService.CreatePending(Guid.NewGuid(), viewmodel.JobItemId, viewmodel.SupplierId, viewmodel.Instructions);
					transaction.Commit();
					return RedirectToAction("Details", "JobItem", new { Id = viewmodel.JobItemId });
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
			return PartialView("_Create", viewmodel);
		}

		[HttpGet]
		public ActionResult EditPending(Guid id)
		{
			var consignment = _consignmentItemService.GetPendingItem(id);

			var viewmodel = new ConsignmentEditViewModel()
			{
				Id = consignment.Id,
				JobItemId = consignment.JobItem.Id,
				SupplierName = consignment.Supplier.Name,
				SupplierId = consignment.Supplier.Id,
				Instructions = consignment.Instructions
			};
			return PartialView("_Edit", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditPending(ConsignmentEditViewModel viewmodel)
		{
			_consignmentItemService.EditPending(
				viewmodel.Id,
				viewmodel.JobItemId,
				viewmodel.SupplierId,
				viewmodel.Instructions);

			return RedirectToAction("PendingConsignments", "Consignment");
		}

		[Transaction]
		public ActionResult ConvertToOrder(Guid id)
		{
			_orderService.CreateOrderFromConsignment(id);

			return RedirectToAction("ActiveConsignments", "Consignment");
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
			var viewmodel = new ConsignmentPendingListViewModel()
			{
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

		public ActionResult ConsignPending(Guid[] ToBeConvertedIds)
		{
			if (ModelState.IsValid)
			{
				if (ToBeConvertedIds.Length > 0)
				{
					var transaction = NHibernateSession.Current.BeginTransaction();
					try
					{
						_consignmentService.CreateConsignmentsFromPendingItems(ToBeConvertedIds);
						transaction.Commit();
					}
					catch (DomainValidationException dex)
					{
						ModelState.UpdateFromDomain(dex.Result);
						transaction.Rollback();
					}
					finally
					{
						transaction.Dispose();
					}
				}
			}
			return RedirectToAction("PendingConsignments");
		}

		public ActionResult GenerateConsignmentNote(Guid id)
		{
			return View("RepConsignmentNote", id);
		}
	}
}
