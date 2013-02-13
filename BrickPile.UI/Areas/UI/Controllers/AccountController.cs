/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BrickPile.UI.Areas.UI.Models;

namespace BrickPile.UI.Areas.UI.Controllers {
    
	public class AccountController : Controller {

		public IFormsAuthenticationService FormsService { get; set; }
		public IMembershipService MembershipService { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
			if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

			base.Initialize(requestContext);
		}

		// **************************************
		// URL: /Account/LogOn
		// **************************************
		public ActionResult Index() {
			return View("LogOn");
		}

		[HttpPost]
		public ActionResult Index(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid) {
			    if (MembershipService.ValidateUser(model.UserName, model.Password))
				{
					FormsService.SignIn(model.UserName, model.RememberMe);

					if (Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
				    return RedirectToAction("Index", "Dashboard");
				}
			    ModelState.AddModelError("", "The user name or password provided is incorrect.");
			}

		    // If we got this far, something failed, redisplay form
			return View("LogOn", model);
		}

		// **************************************
		// URL: /Account/LogOff
		// **************************************

		public ActionResult LogOff()
		{
			FormsService.SignOut();
		    return RedirectToAction("Index", "Home", new { area = "" });
		}

		// **************************************
		// URL: /Account/Register
		// **************************************

		public ActionResult Register()
		{
            ViewBag.Class = "register";
			ViewBag.PasswordLength = MembershipService.MinPasswordLength;
			return View();
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

				if (createStatus == MembershipCreateStatus.Success)
				{
					FormsService.SignIn(model.UserName, false /* createPersistentCookie */);

					return RedirectToAction("index", "Content",new { area = "UI" });
				}
				else
				{
					ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
				}
			}

			// If we got this far, something failed, redisplay form

		    ViewBag.Class = "register";
			ViewBag.PasswordLength = MembershipService.MinPasswordLength;
			return View(model);
		}

		// **************************************
		// URL: /Account/ChangePassword
		// **************************************

		[Authorize]
		public ActionResult ChangePassword()
		{
			ViewBag.PasswordLength = MembershipService.MinPasswordLength;
			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			ViewBag.PasswordLength = MembershipService.MinPasswordLength;
			return View(model);
		}

		// **************************************
		// URL: /Account/ChangePasswordSuccess
		// **************************************

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		public ActionResult ManageUsers() {
		    ViewBag.Class = "users";
			var users = MembershipService.GetAllUsers();
			return View( users );
		}

		public ActionResult ManageRoles()
		{
			var roles = MembershipService.GetAllRoles();
			return View(roles);
		}

		[HttpPost]
		public ActionResult ManageRoles(string roleName)
		{
			if (String.IsNullOrEmpty(roleName))
			{
				ModelState.AddModelError("roleName", "Name is required");
				return ManageRoles();
			}
			else
			{
				MembershipService.AddRole(roleName);
				return RedirectToAction("ManageRoles");
			}

		}

		public ActionResult EditUser(string username)
		{
			var user = MembershipService.GetUser(username);
			var roles = MembershipService.GetAllRoles();
			var userRoles = MembershipService.GetRolesForUser(user.UserName);

			return View(new EditUserModel(user.UserName, user.Email, roles, userRoles));
		}

		[HttpPost]
		public ActionResult EditUser(EditUserModel model)
		{
			var user = MembershipService.GetUser(model.Username);
			MembershipService.UpdateUser(user, model.UserRoles);
			return RedirectToAction("ManageUsers");
		}

		[HttpPost]
		public ActionResult DeleteRole(string roleName)
		{
			MembershipService.DeleteRole(roleName);
			return RedirectToAction("ManageRoles");
		}
	}
}
