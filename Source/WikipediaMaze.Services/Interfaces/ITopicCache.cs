using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface ITopicCache
    {
        string GetTopicHtml(string name);
        void SetTopicHtml(string name, string html);
        bool TopicExists(string name);

        Topic GetTopic(string name);
        void SetTopic(string name, Topic topic);
    }
}
