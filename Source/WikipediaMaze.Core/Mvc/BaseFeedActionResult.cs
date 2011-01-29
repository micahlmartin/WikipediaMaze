using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace WikipediaMaze.Core.Mvc
{
    public abstract class BaseFeedActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        protected abstract string ContentType { get; }
        protected abstract SyndicationFeedFormatter Formatter { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = ContentType;
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                Formatter.WriteTo(writer);
            }
        }
    }
}
