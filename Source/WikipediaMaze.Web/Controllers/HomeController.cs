using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Filters;
using WikipediaMaze.Controllers.SubControllers;
using WikipediaMaze.Data;
using StructureMap;
using WikipediaMaze.Core;
using MvcContrib;
using WikipediaMaze.Services;
using log4net;
using MvcContrib.Pagination;
using WikipediaMaze.App;
using WikipediaMaze.ViewModels;

namespace WikipediaMaze.Controllers
{
    [HandleErrorWithElmah]
    [SubControllerActionToViewData]
    public class HomeController : Controller
    {
        #region Fields

        private readonly IPuzzleService _puzzleService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors

        public HomeController(IPuzzleService puzzleService, IAuthenticationService authenticationService, IAccountService accountService)
        {
            _puzzleService = puzzleService;
            _authenticationService = authenticationService;
            _accountService = accountService;
        }

        #endregion

        public ActionResult Index(HeaderInfoController headerInfoController, SidebarController sidebarController, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            sortType = sortType ?? PuzzleSortType.Newest;
            page = page ?? 1;
            pageSize = pageSize ?? 15;

            //Get sorted puzzles
            var puzzles = _puzzleService.GetPuzzleDetailView(sortType.Value, page.Value, pageSize.Value);

            var viewModel = new PuzzleListViewModel("Welcome", puzzles, sortType.Value, _authenticationService.IsAuthenticated, _authenticationService.CurrentUserId);

            return View(viewModel);
        }

        public ActionResult About(HeaderInfoController headerInfoController)
        {
            return View();
        }

        public ActionResult HowToPlay(HeaderInfoController headerInfoController)
        {
            if (!_authenticationService.IsAuthenticated)
            {
                if (Request.Cookies["ShowNotification"] != null)
                    Request.Cookies["ShowNotification"].Value = bool.TrueString;
                else
                {
                    var cookie = new HttpCookie("ShowNotification", bool.FalseString);
                    cookie.Domain = Settings.Host;
                    Response.AppendCookie(cookie);
                }
            }

            return View();
        }

        public ActionResult Offline()
        {
            return Settings.IsOffline ? View() : View("NotFound");
        }

        public ActionResult Heartbeat()
        {
            return new EmptyResult();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _log.Error(filterContext.Exception.Message, filterContext.Exception);
        }
        
    }
}
