using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Customers;

namespace JobSystem.Mvc.Controllers
{
	public class CustomerController : Controller
	{
		private CustomerService _customerService;

		public CustomerController(CustomerService customerService)
		{
			_customerService = customerService;
		}

		public ActionResult Index(int page = 1)
		{
			var pageSize = 15;
			var customerList = _customerService.GetCustomers().Select(
				c => new CustomerIndexViewModel
				{
					Id = c.Id,
					Name = c.Name,
					AssetLine = c.AssetLine,
					Email = c.Email,
					Contact1 = c.Contact1
				}).OrderBy(c => c.Name).Skip((page - 1) * pageSize).Take(pageSize);
			return View(new CustomerListViewModel
			{
				Customers = customerList,
				Page = page,
				PageSize = pageSize,
				Total = _customerService.GetCustomersCount()
			});
		}

		public ActionResult Details(Guid id)
		{
			var customer = _customerService.GetById(id);
			return View(CustomerViewModel.GetCustomerViewModelFromCustomer(customer));
		}

		public ActionResult Create()
		{
			return View(new CustomerViewModel());
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(CustomerViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_customerService.Create(
						id,
						viewModel.Name,
						viewModel.AssetLine, 
						Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5),
						ContactInfo.GetContactInfoFromDetails(viewModel.Telephone, viewModel.Fax, viewModel.Email, viewModel.Contact1, viewModel.Contact2),
						viewModel.InvoiceTitle,
						viewModel.InvoiceAddressSameAsMain ?
							Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5) :
							Address.GetAddressFromLineDetails(viewModel.InvoiceAddress1, viewModel.InvoiceAddress2, viewModel.InvoiceAddress3, viewModel.InvoiceAddress4, viewModel.InvoiceAddress5),
						ContactInfo.GetContactInfoFromDetails(viewModel.SalesTelephone, viewModel.SalesFax, viewModel.SalesEmail, viewModel.SalesContact1, viewModel.SalesContact2),
						viewModel.DeliveryTitle,
						viewModel.DeliveryAddressSameAsMain ?
							Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5) :
							Address.GetDeliveryAddressFromLineDetails(viewModel.DeliveryAddress1, viewModel.DeliveryAddress2, viewModel.DeliveryAddress3, viewModel.DeliveryAddress4, viewModel.DeliveryAddress5, viewModel.DeliveryAddress6, viewModel.DeliveryAddress7),
						ContactInfo.GetContactInfoFromDetails(viewModel.DeliveryTelephone, viewModel.DeliveryFax, viewModel.DeliveryEmail, viewModel.DeliveryContact1, viewModel.DeliveryContact2));
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
			var customer = _customerService.GetById(id);
			return View(CustomerViewModel.GetCustomerViewModelFromCustomer(customer));
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(CustomerViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_customerService.Edit(
						viewModel.Id,
						viewModel.Name,
						viewModel.AssetLine, 
						Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5),
						ContactInfo.GetContactInfoFromDetails(viewModel.Telephone, viewModel.Fax, viewModel.Email, viewModel.Contact1, viewModel.Contact2),
						viewModel.InvoiceTitle,
						viewModel.InvoiceAddressSameAsMain ? Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5) : Address.GetAddressFromLineDetails(viewModel.InvoiceAddress1, viewModel.InvoiceAddress2, viewModel.InvoiceAddress3, viewModel.InvoiceAddress4, viewModel.InvoiceAddress5),
						ContactInfo.GetContactInfoFromDetails(viewModel.SalesTelephone, viewModel.SalesFax, viewModel.SalesEmail, viewModel.SalesContact1, viewModel.SalesContact2),
						viewModel.DeliveryTitle,
						viewModel.DeliveryAddressSameAsMain ? Address.GetAddressFromLineDetails(viewModel.Address1, viewModel.Address2, viewModel.Address3, viewModel.Address4, viewModel.Address5) : Address.GetAddressFromLineDetails(viewModel.DeliveryAddress1, viewModel.DeliveryAddress2, viewModel.DeliveryAddress3, viewModel.DeliveryAddress4, viewModel.DeliveryAddress5),
						ContactInfo.GetContactInfoFromDetails(viewModel.DeliveryTelephone, viewModel.DeliveryFax, viewModel.DeliveryEmail, viewModel.DeliveryContact1, viewModel.DeliveryContact2));
					return RedirectToAction("Index");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult SearchCustomers(string query)
		{
			IEnumerable<Customer> customers = _customerService.SearchByKeyword(query);
			return Json(customers);
		}
	}
}