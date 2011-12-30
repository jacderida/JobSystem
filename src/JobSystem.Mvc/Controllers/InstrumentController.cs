using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
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

    }
}
