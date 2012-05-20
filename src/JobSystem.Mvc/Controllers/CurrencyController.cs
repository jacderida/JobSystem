using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Currencies;

namespace JobSystem.Mvc.Controllers
{
	public class CurrencyController : Controller
	{
		private CurrencyService _currencyService;

		public CurrencyController(CurrencyService currencyService)
		{
			_currencyService = currencyService;
		}

		public ActionResult Index()
		{
			var currencies = _currencyService.GetCurrencies().Select(c => new CurrencyViewModel { Id = c.Id, Name = c.Name, DisplayMessage = c.DisplayMessage }).ToList();
			var listViewModel = new CurrencyListViewModel()
			{
				Currencies = currencies,
				CreateViewModel = new CurrencyViewModel()
			};
			return View(listViewModel);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(CurrencyViewModel viewmodel)
		{
			try
			{
				_currencyService.Create(Guid.NewGuid(), viewmodel.Name, viewmodel.DisplayMessage);
			}
			catch (DomainValidationException dex)
			{
				ModelState.UpdateFromDomain(dex.Result);
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(Guid id)
		{
			var currency = _currencyService.GetById(id);
			return PartialView("_Edit", new CurrencyViewModel
			{
				Id = id,
				Name = currency.Name,
				DisplayMessage = currency.DisplayMessage
			});
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(CurrencyViewModel model)
		{
			try
			{
				_currencyService.Edit(model.Id, model.Name, model.DisplayMessage);
			}
			catch (DomainValidationException dex)
			{
				ModelState.UpdateFromDomain(dex.Result);
			}
			return RedirectToAction("Index");
		}
	}
}