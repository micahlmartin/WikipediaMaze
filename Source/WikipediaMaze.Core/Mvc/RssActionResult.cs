using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace WikipediaMaze.Core.Mvc
{
    public class RssActionResult : BaseFeedActionResult
    {
        protected override string ContentType
        {
            get { return "application/rss+xml"; }
        }

        protected override SyndicationFeedFormatter Formatter
        {
            get { return new Rss20FeedFormatter(Feed); }
        }
    }
}