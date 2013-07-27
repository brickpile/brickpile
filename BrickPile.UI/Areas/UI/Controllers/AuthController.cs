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

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Mvc;
using BrickPile.UI.Areas.UI.Models;
using BrickPile.UI.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Contrib.AspNet.Auth;
using Raven.Contrib.AspNet.Auth.Interfaces;
using Raven.Contrib.AspNet.Auth.Providers;
using StructureMap;

namespace BrickPile.UI.Areas.UI.Controllers {

    [System.Web.Mvc.Authorize]
    public class AuthController : ApiController {

        //public enum ManageMessageId {
        //    ChangePasswordSuccess,
        //    SetPasswordSuccess,
        //    RemoveLoginSuccess
        //}

        protected readonly IAuthProvider Authenticator;

        private readonly IDictionary<string, IExternalAuthProvider> _providers;

        private IDocumentSession _session = ObjectFactory.GetInstance<IDocumentSession>();

        public AuthController() {

            Authenticator = new AuthProvider();

            //var google = new GoogleAuthenticator(new ClaimsRequest { Nickname = DemandLevel.Request });
            var facebook = new FacebookAuthenticator("265895203421271", "ef99fdfd9ec9b2ff32389c1606c9ba63");
            //var twitter = new TwitterAuthenticator("Vy6v64XRgBbm3pytUq1w", "TL3jZSxynoXJJDUmKOVdjcQw2fcGc4hZDm7ySYc");

            _providers = new Dictionary<string, IExternalAuthProvider>
            {
                //{ google.ProviderName.ToLowerInvariant(), google },
                { facebook.ProviderName.ToLowerInvariant(), facebook }
                //{ twitter.ProviderName.ToLowerInvariant(), twitter }
                // Add Facebook, Twitter, etc.
            };
        }

        //
        // GET: /Account/Login

        //[System.Web.Mvc.AllowAnonymous]
        //public ActionResult Login(string returnUrl) {
        //    return View(new LoginModel
        //    {
        //        ReturnUrl = returnUrl
        //    });
        //}

        //
        // POST: /Account/Login

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public HttpResponseMessage Login([FromBody]LoginModel model) {
            try {
                if (ModelState.IsValid) {
                    Authenticator.Login(model.UserName, model.Password, model.RememberMe);
                    var jsonSerializerSettings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { username = Authenticator.Current }, jsonSerializerSettings)),
                        StatusCode = HttpStatusCode.OK
                    };
                }
            } catch (InvalidCredentialException) {
                return new HttpResponseMessage
                {
                    Content = new StringContent("The user name or password provided is incorrect."),
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        //
        // POST: /Account/LogOff

        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        public HttpResponseMessage LogOff() {
            Authenticator.Logout();
            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK
            };
        }

        //
        // GET: /Account/Register

        //[AllowAnonymous]
        //public ActionResult Register() {
        //    return View();
        //}

        //
        // POST: /Account/Register

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public HttpResponseMessage Post([FromBody]RegisterModel model) {
            if (ModelState.IsValid) {
                // Attempt to register the user
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                try {
                    Authenticator.CreateAccount(model.UserName, model.Password);
                    Authenticator.Login(model.UserName, model.Password);

                    IConfiguration configuration = new Configuration.Configuration();
                    _session.Store(configuration);
                    _session.SaveChanges();

                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { username = Authenticator.Current }, jsonSerializerSettings)),
                        StatusCode = HttpStatusCode.OK
                    };
                } catch (DuplicateUserNameException) {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("User name already exists. Please enter a different user name.")
                    };
                }
            }
            // If we got this far, something failed, redisplay form
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Session() {
            var jsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            if(Authenticator.IsAuthenticated) {
                //return Request.CreateResponse(HttpStatusCode.OK, new { username = Authenticator.Current });
                return new HttpResponseMessage {
                    Content = new StringContent(JsonConvert.SerializeObject(new { username = Authenticator.Current }, jsonSerializerSettings)),
                    StatusCode = HttpStatusCode.OK
                };
            }

            var config = _session.Load<IConfiguration>("brickpile/configuration");
            if(config == null) {
                //return Request.CreateResponse(HttpStatusCode.Forbidden);
                return new HttpResponseMessage {
                    StatusCode = HttpStatusCode.Forbidden
                };                                
            }

            //return Request.CreateResponse(HttpStatusCode.Unauthorized);
            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.Unauthorized
            };
        }
        //
        // POST: /Account/Disassociate

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Disassociate(ExternalLoginModel model) {
        //    var userName = Authenticator.Current;
        //    var identifiers = Authenticator.GetIdentifiers(userName);

        //    bool isLocal = Authenticator.IsLocalAccount(userName);
        //    bool isOwner = identifiers.Any(i => i.Key == model.ProviderName && i.Value == model.UserIdentifier);

        //    // Only disassociate the account if the currently logged in user
        //    // is the owner and it is not the user's last login credential.
        //    if (isOwner && (isLocal || identifiers.Count > 1))
        //        Authenticator.RemoveIdentifier(userName, model.ProviderName);

        //    return RedirectToAction("Manage");
        //}

        //
        // GET: /Account/Manage

        //public ActionResult Manage(ManageMessageId? message) {
        //    var userName = Authenticator.Current;
        //    var model = new ManageModel
        //    {
        //        UserName = userName,
        //        HasLocalPassword = Authenticator.IsLocalAccount(userName)
        //    };

        //    switch (message) {
        //        case ManageMessageId.ChangePasswordSuccess:
        //            model.StatusMessage = "Your password has been changed.";

        //            break;

        //        case ManageMessageId.SetPasswordSuccess:
        //            model.StatusMessage = "Your password has been set.";

        //            break;

        //        case ManageMessageId.RemoveLoginSuccess:
        //            model.StatusMessage = "The external login was removed.";

        //            break;
        //    }

        //    return View(model);
        //}

        //
        // POST: /Account/Manage

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Manage(ManageModel model) {
        //    var userName = Authenticator.Current;

        //    if (Authenticator.IsLocalAccount(userName)) {
        //        if (ModelState.IsValid) {
        //            try {
        //                Authenticator.ChangePassword(userName, model.OldPassword, model.NewPassword);

        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            } catch (Exception) {
        //                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //            }
        //        }
        //    } else {
        //        // User does not have a local password.
        //        // Remove any validation errors caused by a missing OldPassword field.
        //        ModelState state = ModelState["OldPassword"];

        //        if (state != null)
        //            state.Errors.Clear();

        //        if (ModelState.IsValid) {
        //            Authenticator.SetPassword(userName, model.NewPassword);

        //            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // POST: /Account/ExternalLogin

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public HttpResponseMessage ExternalLogin(string provider, string returnUrl) {

        //    var authProvier = _providers[provider.ToLowerInvariant()];

        //    //var action = Url.Action("ExternalLoginCallback");
        //    string action = "/api/account/ExternalLoginCallback";
            

        //    var callback = Url.Public(action);
            
        //    //Session["ReturnUrl"] = returnUrl;
            
        //    object httpContextBase;
        //    ControllerContext.Request.Properties.TryGetValue("MS_HttpContext", out httpContextBase);

        //    authProvier.RequestAuthentication(httpContextBase as HttpContextBase, callback.AddQueryParameter("provider", provider));

        //    // Show not get here.
        //    return new HttpResponseMessage();
        //}

        //
        // GET: /Account/ExternalLoginCallback

        //[System.Web.Http.AllowAnonymous]
        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage ExternalLoginCallback(string provider) {

        //    var authProvider = _providers[provider.ToLowerInvariant()];

        //    object httpContextBase;
        //    ControllerContext.Request.Properties.TryGetValue("MS_HttpContext", out httpContextBase);

        //    var authResult = authProvider.VerifyAuthentication(httpContextBase as HttpContextBase);

        //    if (authResult.Result != AuthResult.Status.Authenticated) {
        //        return new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.Unauthorized
        //        };
        //    }

        //    try {
        //        Authenticator.Login(authResult.Information.Identifier);

        //    //    return RedirectToLocal((string)Session["ReturnUrl"]);
        //    } catch (Exception) {
        //        if (Authenticator.IsAuthenticated) {
        //    //        // If the current user is logged in add the new identifier.
        //    //        //Authenticator.AddIdentifier(Authenticator.Current, authResult.Information.Identifier, "Google");
        //            Authenticator.AddIdentifier(Authenticator.Current, authResult.Information.Identifier, "Facebook");
        //    //        Authenticator.AddIdentifier(Authenticator.Current, authResult.Information.Identifier, "Facebook");

        //    //        return RedirectToLocal((string)Session["ReturnUrl"]);
        //        } else {
        //            var profile = authResult.Information.Profile;

        //    //        // User is new, ask for their desired username.
        //    //        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel
        //    //        {
        //    //            UserName = profile == null ? null : profile.Nickname,
        //    //            UserIdentifier = authResult.Information.Identifier,
        //    //            ProviderName = authProvider.ProviderName,
        //    //        });

        //            return new HttpResponseMessage
        //            {
        //                StatusCode = HttpStatusCode.OK
        //            };

        //        }
        //    }
        //    return new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.Unauthorized
        //    };

        //}

        //
        // POST: /Account/ExternalLoginConfirmation

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model) {
        //    if (Authenticator.IsAuthenticated) {
        //        return RedirectToAction("Manage");
        //    } else if (ModelState.IsValid) {
        //        Authenticator.CreateExternalAccount(model.UserName, model.UserIdentifier, model.ProviderName);
        //        Authenticator.Login(model.UserIdentifier);

        //        return RedirectToLocal((string)Session["ReturnUrl"]);
        //    } else {
        //        return View(model);
        //    }
        //}

        //
        // GET: /Account/ExternalLoginFailure

        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure() {
        //    return View();
        //}

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl) {
        //    return PartialView("_ExternalLoginsListPartial", new ExternalLoginsListModel
        //    {
        //        ProviderNames = _providers.Keys,
        //        ReturnUrl = returnUrl,
        //    });
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveExternalLogins() {
        //    var userName = Authenticator.Current;
        //    var logins = Authenticator.GetIdentifiers(userName).Select(i => new ExternalLoginModel
        //    {
        //        ProviderName = i.Key,
        //        UserIdentifier = i.Value,
        //    }).ToList();

        //    return PartialView("_RemoveExternalLoginsPartial", new RemoveExternalLoginModel
        //    {
        //        ExternalLogins = logins,
        //        ShowRemoveButton = logins.Any() || Authenticator.IsLocalAccount(userName),
        //    });
        //}
    }    
}
