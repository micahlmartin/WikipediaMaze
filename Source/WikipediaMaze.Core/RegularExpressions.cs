using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WikipediaMaze.Core
{
    public static class RegularExpressions
    {
        public static readonly Regex EmailRegex = new Regex(@"^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$");
        public static readonly Regex UserNameRegex = new Regex(@"^([ \u00c0-\u01ffa-zA-Z'])+$");
        public static readonly Regex WikipediaLinkRegex = new Regex("(?<Url>http://(www|en).wikipedia(game|).(org|com)/wiki/)(?<Topic>.+)");
        public static readonly Regex DisambiguationTopicRegex = new Regex(@"(?<Topic>.+?)(?<Disambiguation>_\(.+?\))");
        public static readonly Regex TopicLinkRegex = new Regex("(?<Prefix>/wiki/)(?<Topic>.+)");
    }
}
