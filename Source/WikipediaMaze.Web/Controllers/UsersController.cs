using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using MvcContrib.Filters;
using MvcContrib.Pagination;
using WikipediaMaze.App;
using WikipediaMaze.Controllers.SubControllers;
using WikipediaMaze.Core;
using WikipediaMaze.Core.Mvc;
using WikipediaMaze.Services;
using WikipediaMaze.ViewModels;
using WikipediaMaze.Web;

namespace WikipediaMaze.Controllers
{
    [HandleErrorWithElmah]
    public class UsersController : Controller
    {
        public ActionResult Index(PlayerSortType? sortType, int? page)
        {
            var urlHelper = new UrlHelper(new RequestContext(HttpContext, RouteData), RouteTable.Routes );
            var url = urlHelper.Action("Index", "Players", new RouteValueDictionary {{"sortType", sortType}, {"page", page}},
                             "http", "wikipediamaze.com");

            return new PermanentRedirectResult(url);
        }

        public ActionResult Display(int id, string userName, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            var urlHelper = new UrlHelper(new RequestContext(HttpContext, RouteData), RouteTable.Routes);
            var url = urlHelper.Action("Display", "Players", new RouteValueDictionary { { "id", id }, { "userName", page } , {"sortType", sortType},{"page", page},{"pageSize", pageSize}},
                             "http", "wikipediamaze.com");

            return new PermanentRedirectResult(url);
        }

        public ActionResult Edit(int id)
        {
            var urlHelper = new UrlHelper(new RequestContext(HttpContext, RouteData), RouteTable.Routes);
            var url = urlHelper.Action("Edit", "Players", new RouteValueDictionary {{"id", id}}, "http",
                                       "wikipediamaze.com");

            return new PermanentRedirectResult(url);
        }

        public ActionResult UserDisplayPuzzles(int id, PuzzleSortType? sortType, int? page, int? pageSize)
        {
            var urlHelper = new UrlHelper(new RequestContext(HttpContext, RouteData), RouteTable.Routes);
            var url = urlHelper.Action("UserDisplayPuzzles", "Players", new RouteValueDictionary { { "id", id }, { "sortType", sortType }, { "page", page }, { "pageSize", pageSize } },
                             "http", "wikipediamaze.com");

            return new PermanentRedirectResult(url);
        }

        public ActionResult UserDisplaySolutions(int id, SolutionSortType? sortType, int? page)
        {
            var urlHelper = new UrlHelper(new RequestContext(HttpContext, RouteData), RouteTable.Routes);
            var url = urlHelper.Action("UserDisplaySolutions", "Players", new RouteValueDictionary { { "id", id }, { "sortType", sortType }, { "page", page }},
                             "http", "wikipediamaze.com");

            return new PermanentRedirectResult(url);
        }
    }
}
