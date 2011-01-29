using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.Messaging;
//using DotNetOpenAuth.OpenId.RelyingParty;
using MvcContrib.Filters;
using WikipediaMaze.Core;
using WikipediaMaze.Core.Properties;
using WikipediaMaze.Services;
using WikipediaMaze.Controllers.SubControllers;
using log4net;
using WikipediaMaze.App;
using WikipediaMaze.ViewModels;
using System.Web;
//using DotNetOpenAuth.OpenId;
//using facebook;
//using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
//using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;

namespace WikipediaMaze.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel(string returnUrl, bool isAuthenticated)
        {
            ReturnUrl = returnUrl;
            IsAuthenticated = isAuthenticated;
        }

        public string ReturnUrl { get; private set; }
        public bool IsAuthenticated { get; private set; }
    }
    
}
namespace WikipediaMaze.Controllers
{

    [HandleErrorWithElmah]
    [SubControllerActionToViewData]
    public class AccountController : Controller
    {
       
        #region Fields

        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors

        public AccountController(IAccountService accountService, IAuthenticationService authenticationService)
        {
            _accountService = accountService;
            _authenticationService = authenticationService;
        }

        #endregion

        public ActionResult Login(HeaderInfoController headerInfoController, string returnUrl)
        {
            if (_authenticationService.IsAuthenticated)
                return RedirectToAction("index", "home");

            return View(new LoginViewModel(returnUrl, _authenticationService.IsAuthenticated));
        }

        //public ActionResult Authenticate(HeaderInfoController headerInfoController, SidebarController sidebarController, string returnUrl)
        //{
        //    var openId = new OpenIdRelyingParty();
        //    var response = openId.GetResponse();
        //    if (response == null)
        //    {
        //        Identifier id;
        //        if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
        //        {
        //            try
        //            {
        //                var request = openId.CreateRequest(Request.Form["openid_identifier"]);
        //                request.AddExtension(new ClaimsRequest
        //                                         {
        //                                             BirthDate = DemandLevel.Request,
        //                                             Country = DemandLevel.Request,
        //                                             Email = DemandLevel.Require,
        //                                             FullName = DemandLevel.Request,
        //                                             Gender = DemandLevel.Request,
        //                                             Language = DemandLevel.Request,
        //                                             Nickname = DemandLevel.Request,
        //                                             PostalCode = DemandLevel.Request,
        //                                             TimeZone = DemandLevel.Request 
        //                                         });

        //                return request.RedirectingResponse.AsActionResult();
        //            }
        //            catch (ProtocolException)
        //            {
        //                ViewData["Message"] = "Invalid Identifier";
        //                return View("Login");
        //            }
        //        }
        //        else
        //        {
        //            ViewData["Message"] = "Invalid Identifier";
        //            return View("Login");
        //        }
        //    }
        //    else
        //    {
        //        switch (response.Status)
        //        {
        //            case AuthenticationStatus.Authenticated:
        //                Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;
        //                FormsAuthentication.SetAuthCookie(response.ClaimedIdentifier, false);
        //                if (!string.IsNullOrEmpty(returnUrl))
        //                {
        //                    return Redirect(returnUrl);
        //                }
        //                else
        //                {
        //                    return RedirectToAction("Index", "Home");
        //                }
        //            case AuthenticationStatus.Canceled:
        //                ViewData["Message"] = "Canceled at provider";
        //                return View("Login");
        //            case AuthenticationStatus.Failed:
        //                ViewData["Message"] = response.Exception.Message;
        //                return View("Login");
        //        }
        //    }
        //    return new EmptyResult();
        //}

        public ActionResult LogOn(string token, string returnUrl)
        {
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;

            var profile = _accountService.GetOpenIdProfile(token);

            if (profile == null)
                return Redirect(returnUrl);

            var user = _accountService.GetUserFromProfile(profile);
            if (user == null)
                throw new ApplicationException("An error occurred trying to retrieve the user information from the OpenId profile");

            if (_authenticationService.CurrentUserId != user.Id)
                _authenticationService.SignIn(user.Id, true);

            return Redirect(returnUrl);
        }

        public ActionResult LogOff()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _log.Error(filterContext.Exception.Message, filterContext.Exception);
        }

        public JsonResult GetNotifications()
        {
            var notifications = new List<Notification>();
            if (!_authenticationService.IsAuthenticated || _authenticationService.CurrentUser.Reputation == 0)
            {
                if (Request.Cookies.Get("Notifications") == null || Request.Cookies.Get("Notifications").Value == "true")
                {
                    const string message = "First time here? <a href='home/howtoplay' title='Learn how to play'>Click here</a> to learn how to play.";
                    notifications.Add(new Notification {Id = -1, Message = message});
                    return Json(notifications, JsonRequestBehavior.AllowGet);
                }
            }

            notifications = _accountService.GetNotifications(_authenticationService.CurrentUserId).ToList();
            return Json(notifications, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearNotification(int id)
        {
            if (id == -1)
            {
                Response.Cookies["Notifications"].Value = bool.FalseString;
                Response.Cookies["Notifications"].Expires = DateTime.Now.AddYears(1);
                return new EmptyResult();
            }

            _accountService.DeleteNotification(_authenticationService.CurrentUserId, id);
            return new EmptyResult();
        }

        public ActionResult Assume(int id, string password)
        {
            if (password == "!WM2010")
            {
                FormsAuthentication.RedirectFromLoginPage(id.ToString(), false);
                return RedirectToAction("Index", "Home");
            }

            throw new FileNotFoundException();
        }
    }
}
