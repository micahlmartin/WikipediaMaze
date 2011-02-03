using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace WikipediaMaze.Core.Web
{
    public class RemoveWWWPrefixModule : IHttpModule
    {
        public void Dispose() { }

        private static Regex regex = new Regex("(http|https)://www\\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            Uri url = application.Context.Request.Url;
            bool hasWWW = regex.IsMatch(url.ToString());

            if (hasWWW)
            {
                String newUrl = regex.Replace(url.ToString(),
                String.Format("{0}://", url.Scheme));
                application.Context.Response.RedirectPermanent(newUrl);
            }
        }
    }
}
