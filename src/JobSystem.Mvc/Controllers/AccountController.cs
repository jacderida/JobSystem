using System;
using System.Web.Mvc;
using System.Web.Security;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Framework.Security;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.ViewModels.Account;

namespace JobSystem.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManagementService _userManagementService;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public AccountController(UserManagementService userManagementService, IFormsAuthenticationService formsAuthenticationService)
        {
            _userManagementService = userManagementService;
            _formsAuthenticationService = formsAuthenticationService;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_userManagementService.Login(model.UserName, model.Password))
                    {
                        _formsAuthenticationService.SignIn(model.UserName, model.RememberMe);
                        return RedirectToAction("Index", "Job");
                    }
                }
                catch (DomainValidationException dex)
                {
                    ModelState.UpdateFromDomain(dex.Result);
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            _formsAuthenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}