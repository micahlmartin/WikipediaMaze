using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using log4net;
using WikipediaMaze.App;

namespace WikipediaMaze.Controllers
{
    [HandleErrorWithElmah]
    public class ErrorController : Controller
    {
        public ActionResult Unknown()
        {
            return View();
        }
        public ActionResult Forbidden()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult ServerError()
        {
            return View();
        }
        public ActionResult Unavailable()
        {
            return View();   
        }
    }
}
