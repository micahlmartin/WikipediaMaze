using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    [Serializable]
    public class Topic
    {
        public virtual string Name { get; set; }
        public virtual IEnumerable<string> RelatedTopics { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string PageTitle { get; set; }
    }
}
