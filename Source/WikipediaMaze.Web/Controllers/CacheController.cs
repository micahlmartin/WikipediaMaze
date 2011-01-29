using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikipediaMaze.Core.Mvc;

namespace WikipediaMaze.Web.Controllers
{
    public class CacheController : Controller
    {
        public CacheResult CacheContent(string key, string version, string type)
        {
            return new CacheResult(key, version, type);
        }
    }

    public static class CacheControllerExtensions   
    {
        public static CacheResult RenderCacheResult(string keyname, string version, string type)
        {
            return new CacheResult(keyname, version, type);
        }
    }
}