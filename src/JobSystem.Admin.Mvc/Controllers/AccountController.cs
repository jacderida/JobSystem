using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using JobSystem.Admin.Mvc.Models;
using JobSystem.Admin.Mvc.Data;
using JobSystem.Framework.Security;

namespace JobSystem.Admin.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public AccountController(IUserAccountRepository userAccountRepository, IFormsAuthenticationService formsAuthenticationService)
        {
            _userAccountRepository = userAccountRepository;
            _formsAuthenticationService = formsAuthenticationService;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_userAccountRepository.Login(model.UserName, model.Password))
                    {
                        _formsAuthenticationService.SignIn(model.UserName, model.RememberMe);
                        return RedirectToAction("Index", "Tenant");
                    }
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            _formsAuthenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}