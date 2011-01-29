using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WikipediaMaze.Services;

namespace WikipediaMaze.Controllers
{
    public class OAuthController : Controller
    {
        #region Fields

        ITwitterService _twitterService;

        #endregion

        #region Constructors

        public OAuthController(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        #endregion


        public ActionResult AuthorizeTwitter()
        {
            _twitterService.RequestAuthorization();
            return new EmptyResult();
        }

        public JsonResult HasTwitterAuthorization()
        {
            return Json(_twitterService.IsAuthorized, JsonRequestBehavior.AllowGet);
        }
    }
}
