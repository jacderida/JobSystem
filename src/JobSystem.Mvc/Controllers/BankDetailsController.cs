using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.BankDetails;

namespace JobSystem.Mvc.Controllers
{
	public class BankDetailsController : Controller
	{
		private readonly BankDetailsService _bankDetailsService;
		private readonly CompanyDetailsService _companyDetailsService;
		private readonly CurrencyService _currencyService;
		private readonly ListItemService _listItemService;

		public BankDetailsController(
			BankDetailsService bankDetailsService, CompanyDetailsService companyDetailsService, CurrencyService currencyService, ListItemService listItemService)
		{
			_bankDetailsService = bankDetailsService;
			_currencyService = currencyService;
			_companyDetailsService = companyDetailsService;
			_listItemService = listItemService;
		}

		public ActionResult Index()
		{
			var banks = _companyDetailsService.GetBankDetails();
			var viewmodels = new List<BankDetailsIndexViewModel>();
			foreach (var bank in banks)
			{
				viewmodels.Add(new BankDetailsIndexViewModel()
				{
					AccountNo = bank.AccountNo,
					Address1 = bank.Address1,
					Address2 = bank.Address2,
					Address3 = bank.Address3,
					Address4 = bank.Address4,
					Address5 = bank.Address5,
					Iban = bank.Iban,
					Id = bank.Id,
					Name = bank.Name,
					ShortName = bank.ShortName,
					SortCode = bank.SortCode
				});
			}
			return View(viewmodels);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(BankDetailsCreateViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_bankDetailsService.Create(
						id,
						viewModel.Name,
						viewModel.ShortName,
						viewModel.AccountNo,
						viewModel.SortCode,
						viewModel.Address1,
						viewModel.Address2,
						viewModel.Address3,
						viewModel.Address4,
						viewModel.Address5,
						viewModel.Iban);
					return RedirectToAction("Index");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View();
		}

		[HttpGet]
		public ActionResult Edit(Guid id)
		{
			var bankDetails = _bankDetailsService.GetById(id);
			var viewmodel = new BankDetailsCreateViewModel
			{
				Id = bankDetails.Id,
				AccountNo = bankDetails.AccountNo,
				Address1 = bankDetails.Address1,
				Address2 = bankDetails.Address2,
				Address3 = bankDetails.Address3,
				Address4 = bankDetails.Address4,
				Address5 = bankDetails.Address5,
				Iban = bankDetails.Iban,
				Name = bankDetails.Name,
				ShortName = bankDetails.ShortName,
				SortCode = bankDetails.SortCode
			};
			return View(viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(BankDetailsCreateViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_bankDetailsService.Edit(
						viewModel.Id,
						viewModel.Name,
						viewModel.ShortName,
						viewModel.AccountNo,
						viewModel.SortCode,
						viewModel.Address1,
						viewModel.Address2,
						viewModel.Address3,
						viewModel.Address4,
						viewModel.Address5,
						viewModel.Iban);
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