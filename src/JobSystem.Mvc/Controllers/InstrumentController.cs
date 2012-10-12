using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Instruments;
using JobSystem.DataAccess.NHibernate.Web;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using System.Text;

namespace JobSystem.Mvc.Controllers
{
	public class InstrumentController : Controller
	{
		private readonly InstrumentService _instrumentService;

		public InstrumentController(InstrumentService instrumentService)
		{
			_instrumentService = instrumentService;
		}

		public ActionResult Index(int page = 1)
		{
			var instruments = _instrumentService.GetInstruments().Select(
				 i => new InstrumentViewModel
				 {
					 Id = i.Id,
					 Description = i.Description,
					 Manufacturer = i.Manufacturer,
					 ModelNo = i.ModelNo,
					 Range = i.Range,
					 CalibrationTime = i.AllocatedCalibrationTime
				 }).ToList();
			var viewModel = new InstrumentListViewModel();
			viewModel.Instruments = instruments;
			viewModel.CreateViewModel = new InstrumentViewModel();
			return View(viewModel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(InstrumentViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_instrumentService.Create(id, viewModel.Manufacturer, viewModel.ModelNo, viewModel.Range, viewModel.Description, viewModel.CalibrationTime);
					return RedirectToAction("Index");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewModel);
		}

		public ActionResult Edit(Guid id)
		{
			var instrument = _instrumentService.GetById(id);
			return PartialView("_Edit", new InstrumentViewModel
				{
					Id = id,
					Description = instrument.Description,
					Manufacturer = instrument.Manufacturer,
					ModelNo = instrument.ModelNo,
					Range = instrument.Range,
					CalibrationTime = instrument.AllocatedCalibrationTime
				});
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(InstrumentViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_instrumentService.Edit(viewModel.Id, viewModel.Manufacturer, viewModel.ModelNo, viewModel.Range, viewModel.Description, viewModel.CalibrationTime);
					return RedirectToAction("Index");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return PartialView("_Edit", viewModel);
		}

		[HttpPost]
		public ActionResult SearchInstruments(string query)
		{
			IEnumerable<Instrument> instruments = _instrumentService.SearchByKeyword(query);
			
			return Json(instruments); 
		}

		[HttpPost]
		public ActionResult SearchManufacturers(string query)
		{
			IEnumerable<string> manufacturers = _instrumentService.SearchManufacturerByKeyword(query);

			return Json(manufacturers);
		}

		[HttpPost]
		public ActionResult SearchModelNumber(string modelNo, string manufacturer)
		{
			IEnumerable<string> returnedModelNo = _instrumentService.SearchModelNoByKeywordFilterByManufacturer(modelNo, manufacturer);

			return Json(returnedModelNo);
		}
	}
}