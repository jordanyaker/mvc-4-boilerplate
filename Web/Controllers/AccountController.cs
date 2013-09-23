using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Boilerplate.Web.Models;
using NLog;
using Boilerplate.Facades;
using Boilerplate.Domain;
using Boilerplate.Web.Services;

namespace Boilerplate.Web.Controllers {
    [Authorize]
    public class AccountController : Controller {
        // -------------------------------------------------------------------------------------
        // Fields
        // -------------------------------------------------------------------------------------
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the <see cref="T:Boilerplate.Facades.IAccountFacade"/> for the controller.
        /// </summary>
        public IAccountFacade Accounts { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="T:Boilerplate.Facades.IAuthenticationService"/> for the controller.
        /// </summary>
        public IAuthenticationService Authentication { get; set; }

        // -------------------------------------------------------------------------------------
        // Actions
        // -------------------------------------------------------------------------------------
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl) {
            if (ModelState.IsValid) {
                string username = HttpUtility.HtmlEncode(model.UserName);
                string password = HttpUtility.HtmlEncode(model.Password);

                FacadeResult<User> result = Accounts.SignIn(username, password);
                if (result.Type == FacadeResultTypes.Success) {
                    var name = "{0};{1}".FormatWith(result.Data.AccountId, result.Data.Id);
                    Authentication.SignIn(name, model.RememberMe);

                    return RedirectToLocal(returnUrl);
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            Authentication.SignOut();

            return RedirectToRoute("Default", new { action = "Index", controller = "Home" });
        }

        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model) {
            if (ModelState.IsValid) {
                string username = HttpUtility.HtmlEncode(model.UserName);
                string password = HttpUtility.HtmlEncode(model.Password);

                FacadeResult<User> result = Accounts.SignUp(username, "", password);
                if (result.Type == FacadeResultTypes.Success) {
                    var name = "{0};{1}".FormatWith(result.Data.AccountId, result.Data.Id);
                    Authentication.SignIn(name);

                    return RedirectToRoute("Default", new { action = "Index", controller = "Home" });
                }

                ModelState.AddModelError("", result.Error.Description);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            } else {
                return RedirectToRoute("Default", new { action = "Index", controller = "Home" });
            }
        }
        #endregion
    }
}
