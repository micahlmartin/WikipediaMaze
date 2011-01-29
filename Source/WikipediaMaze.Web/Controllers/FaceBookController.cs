using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using facebook;
using WikipediaMaze.Core.Properties;

namespace WikipediaMaze.Controllers
{
    public class FaceBookController : Controller
    {
        #region Fields

        private API _api;

        #endregion

        #region Constructors

        public FaceBookController()
        {
            _api = new API();
            _api.ApplicationKey = Settings.Default.FacebookApiKey;
            _api.Secret = Settings.Default.FacebookSecret;
        }

        #endregion

        public ActionResult Index()
        {
            _api.SetAuthenticationToken();
            var authToken = _api.AuthToken;
            var _loginUrl = String.Format("http://www.facebook.com/login.php?api_key={0}&v=1.0&auth_token={1}",
                                                          Settings.Default.FacebookApiKey, authToken);
            return Redirect(_loginUrl);
        }

    }
}
