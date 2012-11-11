using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Consignments;

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
		public ActionResult EditItem(Guid id)
		{
			var pendingConsignment = _consignmentItemService.GetPendingItem(id);

			if (pendingConsignment == null) {
				var activeConsignment = _consignmentItemService.GetById(id);

				var viewmodel = new ConsignmentEditViewModel()
				{
					Id = activeConsignment.Id,
					JobItemId = activeConsignment.JobItem.Id,
					SupplierName = activeConsignment.Consignment.Supplier.Name,
					SupplierId = activeConsignment.Consignment.Supplier.Id,
					Instructions = activeConsignment.Instructions,
					IsPending = false,
					ConsignmentId = activeConsignment.Consignment.Id
				};

				return View("_EditItem", viewmodel);
			} 
			else
			{
				var viewmodel = new ConsignmentEditViewModel()
				{
					Id = pendingConsignment.Id,
					JobItemId = pendingConsignment.JobItem.Id,
					SupplierName = pendingConsignment.Supplier.Name,
					SupplierId = pendingConsignment.Supplier.Id,
					Instructions = pendingConsignment.Instructions,
					IsPending = true
				};

				return View("_EditItem", viewmodel);
			}
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditItem(ConsignmentEditViewModel viewmodel)
		{
			if (viewmodel.IsPending) 
			{
				_consignmentItemService.EditPending(
					viewmodel.Id,
					viewmodel.JobItemId,
					viewmodel.SupplierId,
					viewmodel.Instructions);
				return RedirectToAction("PendingConsignments", "Consignment");
			}
			else
			{
				_consignmentItemService.Edit(
						viewmodel.Id,
						viewmodel.Instructions);
				return RedirectToAction("ConsignmentItems", "Consignment", new { consignmentId = viewmodel.ConsignmentId });
			}
		}

		[HttpGet]
		public ActionResult Edit(Guid id)
		{
			var activeConsignment = _consignmentService.GetById(id);

			var viewmodel = new ConsignmentEditViewModel()
			{
				Id = activeConsignment.Id,
				SupplierName = activeConsignment.Supplier.Name,
				SupplierId = activeConsignment.Supplier.Id
			};
			return View("_Edit", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(ConsignmentEditViewModel viewmodel)
		{
			_consignmentService.Edit(viewmodel.Id, viewmodel.SupplierId);

			return RedirectToAction("ActiveConsignments", "Consignment");
		}

		[Transaction]
		public ActionResult ConvertToOrder(Guid id)
		{
			try
			{
				_orderService.CreateOrderFromConsignment(id);
			}
			catch (DomainValidationException dex)
			{
				ModelState.UpdateFromDomain(dex.Result);
			}
			return RedirectToAction("ActiveConsignments", "Consignment");
		}

		public ActionResult PendingConsignments(int page = 1)
		{
			var items = _consignmentItemService.GetPendingItems().Select(
				c => new ConsignmentItemIndexViewModel
				{
					Id = c.Id,
					JobItemId = c.JobItem.Id,
					JobItemRef = String.Format("{0}/{1}", c.JobItem.Job.JobNo, c.JobItem.ItemNo),
					Instructions = c.Instructions,
					SupplierName = c.Supplier.Name
				}).OrderBy(c => c.JobItemRef).ToList();
			var viewModel = new ConsignmentPendingListViewModel { ConsignmentItems = items };
			return View(viewModel);
		}

		public ActionResult ActiveConsignments(int page = 1)
		{
			var items = _consignmentService.GetConsignments().Select(
				c => new ConsignmentIndexViewModel
				{
					Id = c.Id,
					ConsignmentNo = c.ConsignmentNo,
					CreatedBy = c.CreatedBy.Name,
					DateCreated = c.DateCreated.ToLongDateString() + ' ' + c.DateCreated.ToShortTimeString(),
					SupplierName = c.Supplier.Name,
					IsOrdered = c.IsOrdered
				}).ToList();

			foreach (var item in items)
			{
				var consignmentItems = _consignmentItemService.GetConsignmentItems(item.Id);
				item.ConsignmentItems = consignmentItems.Select(ci => new ConsignmentItemIndexViewModel
						{
							Instructions = ci.Instructions,
							InstrumentDetails = String.Format("{0} - {1} : {2}", ci.JobItem.Instrument.ModelNo, ci.JobItem.Instrument.Manufacturer.ToString(), ci.JobItem.Instrument.Description),
							Id = ci.Id,
							JobItemRef = String.Format("{0}, item {1}", ci.JobItem.Job.JobNo, ci.JobItem.ItemNo)
						}).ToList();
			}
			return View(items);
		}

		public ActionResult ConsignmentItems(Guid consignmentId)
		{
			var consignmentItemViewModels = new List<ConsignmentItemIndexViewModel>();
			foreach (var item in _consignmentItemService.GetConsignmentItems(consignmentId))
			{
				consignmentItemViewModels.Add(new ConsignmentItemIndexViewModel
				{
					Instructions = item.Instructions,
					InstrumentDetails = String.Format("{0} - {1} : {2}", item.JobItem.Instrument.ModelNo, item.JobItem.Instrument.Manufacturer.ToString(), item.JobItem.Instrument.Description),
					Id = item.Id,
					JobItemRef = String.Format("{0}, item {1}", item.JobItem.Job.JobNo, item.JobItem.ItemNo)
				});
			}
			return View(consignmentItemViewModels);
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
