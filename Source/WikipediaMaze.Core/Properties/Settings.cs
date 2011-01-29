using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WikipediaMaze.Core.Properties
{
    public partial class Settings
    {
        public string Host
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.Url.Port != 80)
                        return HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);

                    return HttpContext.Current.Request.Url.GetComponents(UriComponents.Scheme, UriFormat.Unescaped) + "://" + HttpContext.Current.Request.Url.GetComponents(UriComponents.Host, UriFormat.Unescaped);
                }

                return string.Empty;
            }
        }
    }
}
