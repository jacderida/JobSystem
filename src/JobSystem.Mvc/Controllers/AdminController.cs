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
		private readonly ListItemService _listItemService;

		public AdminController(CompanyDetailsService companyDetailsService, ListItemService listItemService)
		{
			_companyDetailsService = companyDetailsService;
			_listItemService = listItemService;
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
				Currencies = _listItemService.GetAllByType(ListItemType.Currency).ToSelectList(),
				PaymentTerms = _listItemService.GetAllByType(ListItemType.PaymentTerm).ToSelectList(),
				TaxCodes = _listItemService.GetTaxCodes().Select(t => new { Id = t.Id, Name = t.TaxCodeName}).ToSelectList(),
				BankDetails = _listItemService.GetBankDetails().Select(t => new { Id = t.Id, Name = t.ShortName }).ToSelectList()
			};
			return View(companyDetailsViewModel);
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
						viewModel.Id, viewModel.Name,
						Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5),
						viewModel.Telephone, viewModel.Fax, viewModel.Email,
						viewModel.Www, viewModel.RegNo, viewModel.VatRegNo,
						viewModel.TermsAndConditions, viewModel.CurrencyId, viewModel.TaxCodeId,
						viewModel.PaymentTermId, viewModel.BankDetailsId);
					return RedirectToAction("Index");	// Redirect somewhere else...
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