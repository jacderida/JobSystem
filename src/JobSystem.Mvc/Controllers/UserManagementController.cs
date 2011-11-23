using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
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
				u => new UserAccountViewModel
				{
					Id = u.Id,
					EmailAddress = u.EmailAddress,
					Name = u.Name,
					JobTitle = u.JobTitle
				}).ToList();
			return View(users);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
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
			return View(
				new UserAccountViewModel
				{
					Id = user.Id,
					EmailAddress = user.EmailAddress,
					Name =  user.Name,
					JobTitle = user.JobTitle
				});
		}

		[HttpPost]
		public ActionResult Edit(UserAccountViewModel viewModel)
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