using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Core.Mvc
{
    public static class FeedActionResultFactory
    {
        public static BaseFeedActionResult GetFeedActionResult(FeedFormat format)
        {
            switch (format)
            {
                case FeedFormat.Rss:
                    return new RssActionResult();
                    break;
                case FeedFormat.Atom:
                    return new AtomActionResult();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format");
            }
        }
    }
}
