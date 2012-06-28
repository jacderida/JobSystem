using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.Mvc.ViewModels.BankDetails;
using System.Collections.Generic;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataAccess.NHibernate;
using System;
using JobSystem.BusinessLogic.Validation.Core;

namespace JobSystem.Mvc.Controllers
{
	public class BankDetailsController : Controller
	{
		private readonly CompanyDetailsService _companyDetailsService;
		private readonly CurrencyService _currencyService;
		private readonly ListItemService _listItemService;

		public BankDetailsController(
			CompanyDetailsService companyDetailsService, CurrencyService currencyService, ListItemService listItemService)
		{
			_currencyService = currencyService;
			_companyDetailsService = companyDetailsService;
			_listItemService = listItemService;
		}

		public ActionResult Index()
		{
			var banks = _companyDetailsService.GetBankDetails();

			List<BankDetailsIndexViewModel> viewmodels = new List<BankDetailsIndexViewModel>();

			foreach (var bank in banks)
			{
				viewmodels.Add(new BankDetailsIndexViewModel(){
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
		public ActionResult Create(BankDetailsCreateViewModel viewModel)
		{
			//if (ModelState.IsValid)
			//{
			//    var transaction = NHibernateSession.Current.BeginTransaction();
			//    try
			//    {
			//        var id = Guid.NewGuid();
			//        _bankDetailsService.Create(id, 
			//            viewModel.AccountNo, 
			//            viewModel.Address1, 
			//            viewModel.Address2, 
			//            viewModel.Address3, 
			//            viewModel.Address4, 
			//            viewModel.Address5, 
			//            viewModel.Iban, 
			//            viewModel.Name,
			//            viewModel.ShortName);
			//        transaction.Commit();
			//        return RedirectToAction("Index");
			//    }
			//    catch (DomainValidationException dex)
			//    {
			//        transaction.Commit();
			//        ModelState.UpdateFromDomain(dex.Result);
			//    }
			//    finally
			//    {
			//        transaction.Dispose();
			//    }
			//}
			return View();
		}

		[HttpGet]
		public ActionResult Edit(Guid id)
		{
			//var bank = _bankService.GetBank(id);

			//var viewmodel = new BankDetailsCreateViewModel()
			//{
			//    AccountNo = bank.AccountNo,
			//        Address1 = bank.Address1,
			//        Address2 = bank.Address2,
			//        Address3 = bank.Address3,
			//        Address4 = bank.Address4,
			//        Address5 = bank.Address5,
			//        Iban = bank.Iban,
			//        Id = bank.Id,
			//        Name = bank.Name,
			//        ShortName = bank.ShortName,
			//        SortCode = bank.SortCode
			//}
			return View();
		}

		[HttpPost]
		public ActionResult Edit(BankDetailsCreateViewModel viewModel)
		{
			//if (ModelState.IsValid)
			//{
			//    var transaction = NHibernateSession.Current.BeginTransaction();
			//    try
			//    {
			//        _bankDetailsService.Edit(viewModel.Id, 
			//            viewModel.AccountNo, 
			//            viewModel.Address1, 
			//            viewModel.Address2, 
			//            viewModel.Address3, 
			//            viewModel.Address4, 
			//            viewModel.Address5, 
			//            viewModel.Iban, 
			//            viewModel.Name,
			//            viewModel.ShortName);
			//        transaction.Commit();
			//        return RedirectToAction("Index");
			//    }
			//    catch (DomainValidationException dex)
			//    {
			//        transaction.Commit();
			//        ModelState.UpdateFromDomain(dex.Result);
			//    }
			//    finally
			//    {
			//        transaction.Dispose();
			//    }
			//}
			return View();
		}
	}
}