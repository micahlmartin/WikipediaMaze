using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace WikipediaMaze.Core.Mvc
{
    public class PermanentRedirectResult : ActionResult
    {
        public string Url { get; set; }

        public PermanentRedirectResult(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url is null or empty", "url");
            }
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = Url;
            context.HttpContext.Response.End();
        }
    } 
}
