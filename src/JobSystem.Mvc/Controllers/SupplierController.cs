using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Dto;
using JobSystem.Mvc.ViewModels.Suppliers;
using JobSystem.Mvc.Core.UIValidation;

namespace JobSystem.Mvc.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierService _supplierService;

		public SupplierController(SupplierService supplierService)
		{
			_supplierService = supplierService;
		}

        public ActionResult Index()
        {
			var suppliers = _supplierService.GetSuppliers().Select(
				 i => new SupplierIndexViewModel
				 {
					 Id = i.Id,
					 Name = i.Name,
					 TradingAddress1 = i.Address1,
					 TradingAddress2 = i.Address2,
					 TradingAddress3 = i.Address3,
					 TradingAddress4 = i.Address4,
					 TradingAddress5 = i.Address5,
					 TradingContact1 = i.Contact1,
					 TradingContact2 = i.Contact2,
					 TradingEmail = i.Email,
					 TradingFax = i.Fax,
					 TradingTelephone = i.Telephone,
					 SalesAddress1 = i.SalesAddress1,
					 SalesAddress2 = i.SalesAddress2,
					 SalesAddress3 =  i.SalesAddress3,
					 SalesAddress4 = i.SalesAddress4,
					 SalesAddress5 = i.SalesAddress5,
					 SalesContact1 = i.SalesContact1,
					 SalesContact2 = i.SalesContact2,
					 SalesEmail = i.SalesEmail,
					 SalesFax = i.SalesFax,
					 SalesTelephone = i.SalesTelephone
				 }).ToList();
			return View(suppliers);
        }

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(SupplierCreateViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_supplierService.Create(
						id,
						viewModel.Name,
						Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5),
						ContactInfo.GetContactInfoFromDetails(viewModel.Telephone, viewModel.Fax, viewModel.Email, viewModel.Contact1, viewModel.Contact2),
						Address.GetAddressFromLineDetails(viewModel.SalesAddress1, viewModel.SalesAddress2, viewModel.SalesAddress3, viewModel.SalesAddress4, viewModel.SalesAddress5),
						ContactInfo.GetContactInfoFromDetails(viewModel.SalesTelephone, viewModel.SalesFax, viewModel.SalesEmail, viewModel.SalesContact1, viewModel.SalesContact2));
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
