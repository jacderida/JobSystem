using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Admin;

namespace JobSystem.Mvc.Controllers
{
	public class AdminController : Controller
	{
		private readonly CompanyDetailsService _companyDetailsService;
		private readonly CurrencyService _currencyService;
		private readonly ListItemService _listItemService;

		public AdminController(
			CompanyDetailsService companyDetailsService, CurrencyService currencyService, ListItemService listItemService)
		{
			_currencyService = currencyService;
			_companyDetailsService = companyDetailsService;
			_listItemService = listItemService;
		}

		public ActionResult Index()
		{
			var model = new CompanyDetailsViewModel();
			return View("CompanyDetails", model);
		}

		[HttpGet]
		public ActionResult EditCompanyDetails()
		{
			var company = _companyDetailsService.GetCompany();
			var companyDetailsViewModel = new CompanyDetailsViewModel
			{
				Id = company.Id,
				Name = company.Name,
				Address1 = !String.IsNullOrEmpty(company.Address1) ? company.Address1 : String.Empty,
				Address2 = !String.IsNullOrEmpty(company.Address2) ? company.Address2 : String.Empty,
				Address3 = !String.IsNullOrEmpty(company.Address3) ? company.Address3 : String.Empty,
				Address4 = !String.IsNullOrEmpty(company.Address4) ? company.Address4 : String.Empty,
				Address5 = !String.IsNullOrEmpty(company.Address5) ? company.Address5 : String.Empty,
				Telephone = company.Telephone,
				Fax = company.Fax,
				Email = company.Email,
				Www = company.Www,
				TermsAndConditions = company.TermsAndConditions,
				RegNo = !String.IsNullOrEmpty(company.RegNo) ? company.RegNo : String.Empty,
				VatRegNo = !String.IsNullOrEmpty(company.VatRegNo) ? company.VatRegNo : String.Empty,
				CurrencyId = company.DefaultCurrency.Id,
				PaymentTermId = company.DefaultPaymentTerm.Id,
				BankDetailsId = company.DefaultBankDetails.Id,
				TaxCodeId = company.DefaultTaxCode.Id,
				Currencies = _currencyService.GetCurrencies().ToSelectList(),
				PaymentTerms = _listItemService.GetAllByCategory(ListItemCategoryType.PaymentTerm).ToSelectList(),
				TaxCodes = _companyDetailsService.GetTaxCodes().Select(t => new { Id = t.Id, Name = t.TaxCodeName }).ToSelectList(),
				BankDetails = _companyDetailsService.GetBankDetails().Select(t => new { Id = t.Id, Name = t.ShortName }).ToSelectList()
			};
			return View("CompanyDetails", companyDetailsViewModel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditCompanyDetails(CompanyDetailsViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_companyDetailsService.Edit(
						viewModel.Name,
						Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5),
						viewModel.Telephone, viewModel.Fax, viewModel.Email,
						viewModel.Www, viewModel.RegNo, viewModel.VatRegNo,
						viewModel.TermsAndConditions, viewModel.CurrencyId, viewModel.TaxCodeId,
						viewModel.PaymentTermId, viewModel.BankDetailsId);

					//Repopulate lists because MVC doesn't preserve them after post
					viewModel.Currencies = _currencyService.GetCurrencies().ToSelectList();
					viewModel.PaymentTerms = _listItemService.GetAllByCategory(ListItemCategoryType.PaymentTerm).ToSelectList();
					viewModel.TaxCodes = _companyDetailsService.GetTaxCodes().Select(t => new { Id = t.Id, Name = t.TaxCodeName }).ToSelectList();
					viewModel.BankDetails = _companyDetailsService.GetBankDetails().Select(t => new { Id = t.Id, Name = t.ShortName }).ToSelectList();

					return View("CompanyDetails", viewModel);
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			viewModel.Currencies = _currencyService.GetCurrencies().ToSelectList();
			viewModel.PaymentTerms = _listItemService.GetAllByCategory(ListItemCategoryType.PaymentTerm).ToSelectList();
			viewModel.TaxCodes = _companyDetailsService.GetTaxCodes().Select(t => new { Id = t.Id, Name = t.TaxCodeName }).ToSelectList();
			viewModel.BankDetails = _companyDetailsService.GetBankDetails().Select(t => new { Id = t.Id, Name = t.ShortName }).ToSelectList();

			return View("CompanyDetails", viewModel);
		}
	}
}