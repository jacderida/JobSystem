using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Users;

namespace JobSystem.Mvc.Controllers
{
	public class UserManagementController : Controller
	{
		private readonly UserManagementService _userManagementService;

		public UserManagementController(UserManagementService userManagementService)
		{
			_userManagementService = userManagementService;
		}

		public ActionResult Index()
		{
			var users = _userManagementService.GetUsers().Select(
				u => new UserAccountListViewModel
				{
					Id = u.Id,
					EmailAddress = u.EmailAddress,
					Name = u.Name,
					JobTitle = u.JobTitle
				}).ToList();

			var model = new UserAccountIndexViewModel();
			model.Users = users;
			model.CreateEditModel = new UserAccountViewModel();
			return View(model);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(UserAccountViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_userManagementService.Create(id, viewModel.Name, viewModel.EmailAddress, viewModel.Password, viewModel.JobTitle);
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
			var user = _userManagementService.GetById(id);
			return PartialView("_Edit", new UserAccountViewModel
				{
					Id = user.Id,
					EmailAddress = user.EmailAddress,
					Name =  user.Name,
					JobTitle = user.JobTitle
				});
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(UserAccountEditViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_userManagementService.Edit(viewModel.Id, viewModel.Name, viewModel.EmailAddress, viewModel.JobTitle);
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