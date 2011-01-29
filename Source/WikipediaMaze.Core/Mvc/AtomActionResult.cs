using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace WikipediaMaze.Core.Mvc
{
    public class AtomActionResult : BaseFeedActionResult
    {
        protected override string ContentType
        {
            get { return "application/atom+xml";  }
        }

        protected override SyndicationFeedFormatter Formatter
        {
            get { return new Atom10FeedFormatter(Feed); }
        }
    }
}
