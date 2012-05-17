using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcContrib.Filters;
using WikipediaMaze.Controllers.SubControllers;
using WikipediaMaze.Core;
using WikipediaMaze.Services;

namespace WikipediaMaze.Controllers
{
    [SubControllerActionToViewData]
    public class BadgesController : Controller
    {
        private readonly IAccountService _accountService;

        public BadgesController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public ActionResult Index(HeaderInfoController headerInfoController, SidebarController sidebarController)
        {
            var badges = Badges.All.OrderBy(x => x.Level);
            return View(badges);
        }

    }
}
