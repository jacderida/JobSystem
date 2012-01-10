using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Instruments;

namespace JobSystem.Mvc.Controllers
{
    public class InstrumentController : Controller
    {
		private readonly InstrumentService _instrumentService;

		public InstrumentController(InstrumentService instrumentService)
		{
			_instrumentService = instrumentService;
		}

        public ActionResult Index()
        {
           var instruments = _instrumentService.GetInstruments().Select(
				i => new InstrumentViewModel
				{
					Id = i.Id.ToString(),
					Description = i.Description,
					Manufacturer = i.Manufacturer,
					ModelNo = i.ModelNo,
					Range = i.Range
				}).ToList();
			var viewModel = new InstrumentListViewModel();
			viewModel.Instruments = instruments;
			viewModel.CreateViewModel = new InstrumentViewModel();
			return View(viewModel);
        }

		[HttpPost]
		public ActionResult Create(InstrumentViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_instrumentService.Create(id, viewModel.Manufacturer, viewModel.ModelNo, viewModel.Range, viewModel.Description);
					return RedirectToAction("Index");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewModel);
		}

    }
}
