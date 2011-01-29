using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface ITopicService
    {
        Topic GetTopicByUrl(string topicUrl);
        Topic GetTopicByName(string topicName);
        bool DoesTopicExist(string topicUrl);
        string GetTopicNameFromUrl(string topicUrl);
        string GetTopicHtml(string name);
        bool TopicContainsRelatedTopic(string currentTopicUrl, string relatedTopicUrl);
    }
}
