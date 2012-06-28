using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Users;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.ViewModels.Shared;
using System.Collections.Generic;

namespace JobSystem.Mvc.Controllers
{
	public class UserManagementController : Controller
	{
		private readonly UserManagementService _userManagementService;
		private readonly ListItemService _listItemService;

		public UserManagementController(UserManagementService userManagementService, ListItemService listItemService)
		{
			_userManagementService = userManagementService;
			_listItemService = listItemService;
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

			var roles = from UserRole s in Enum.GetValues(typeof(UserRole))
						select new { RoleId = (int)s, Name = s.ToString() };

			var roleVms = new List<CheckboxViewModel>();

			foreach (var role in roles)
			{
				roleVms.Add(new CheckboxViewModel()
				{
					Id = role.RoleId,
					IsChecked = false,
					Name = role.Name
				});
			}

			model.CreateEditModel.Roles = roleVms;

			return View(model);
		}

		public ActionResult Create()
		{
			var roles =
				from UserRole s in Enum.GetValues(typeof(UserRole))
				select new { RoleId = (int)s, Name = s.ToString() };
			var viewmodel = new UserAccountViewModel();
			var roleVms = new List<CheckboxViewModel>();
			foreach (var role in roles)
			{
				if (role.Name != "None")
					roleVms.Add(new CheckboxViewModel
					{
						Id = role.RoleId,
						IsChecked = false,
						Name = role.Name
					});
			}
			viewmodel.Roles = roleVms;
			return View(viewmodel);
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
					var roles = GetRoles(viewModel);
					_userManagementService.Create(id, viewModel.Name, viewModel.EmailAddress, viewModel.Password, viewModel.JobTitle, roles);
					return RedirectToAction("Index");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewModel);
		}

		private UserRole GetRoles(UserAccountViewModel viewModel)
		{
			var roles = UserRole.Member;
			foreach (var checklist in viewModel.Roles)
			{
				switch (checklist.Id)
				{
					case (int)UserRole.Admin:
						if (checklist.IsChecked)
							roles |= UserRole.Admin;
						break;
					case (int)UserRole.Manager:
						if (checklist.IsChecked)
							roles |= UserRole.Manager;
						break;
					case (int)UserRole.JobApprover:
						if (checklist.IsChecked)
							roles |= UserRole.JobApprover;
						break;
					case (int)UserRole.OrderApprover:
						if (checklist.IsChecked)
							roles |= UserRole.OrderApprover;
						break;
					case (int)UserRole.Public:
						if (checklist.IsChecked)
							roles |= UserRole.Public;
						break;
					default:
						break;
				}
			}
			return roles;
		}

		public ActionResult Edit(Guid id)
		{
			var user = _userManagementService.GetById(id);

			var roles = from UserRole s in Enum.GetValues(typeof(UserRole))
						select new { RoleId = (int)s, Name = s.ToString() };

			var roleVms = new List<CheckboxViewModel>();

			foreach (var role in roles)
			{
				roleVms.Add(new CheckboxViewModel()
				{
					Id = role.RoleId,
					//IsChecked = (user.Roles[index].SomeStuff) ? true : false,
					Name = role.Name
				});
			}

			return PartialView("_Edit", new UserAccountEditViewModel
				{
					Id = user.Id,
					EmailAddress = user.EmailAddress,
					Name = user.Name,
					JobTitle = user.JobTitle,
					Roles = roleVms
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
			return PartialView("_Edit", viewModel);
		}
	}
}