using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using System.Web;
using log4net;

namespace WikipediaMaze.Services
{
    public class TopicService : ITopicService
    {
        #region Constants
        
        const string BASE_URL = "http://en.wikipedia.org/wiki/";

        #endregion

        #region Fields

        private readonly ITopicCache _topicCache;
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors

        public TopicService(ITopicCache topicCache)
        {
            _topicCache = topicCache;
        }

        #endregion

        /// <summary>
        /// Get's the specified topic from the database.
        /// </summary>
        /// <param name="topicUrl">The url of the topic to retrieve</param>
        public Topic GetTopicByUrl(string topicUrl)
        {
            var match = RegularExpressions.TopicLinkRegex.Match(topicUrl);
            var group = match.Groups["Topic"];
            if (!group.Success)
                return null;

            var topicName = group.Value;

            var topic = _topicCache.GetTopic(topicName);

            if (topic != null && _topicCache.TopicExists(topicName))
                return topic;

            return GetTopicFromWeb(topicUrl);
        }
        public Topic GetTopicByName(string topicName)
        {
            var topic = _topicCache.GetTopic(topicName);

            if (topic != null && _topicCache.TopicExists(topicName))
                return topic;

            var topicUrl = "{0}{1}".ToFormat(BASE_URL, topicName);
            if(!RegularExpressions.WikipediaLinkRegex.IsMatch(topicUrl))
                return null;

            return GetTopicFromWeb(topicUrl);
        }

        private Topic GetTopicFromWeb(string topicUrl)
        {
            var html = GetHtml(topicUrl);
            if (string.IsNullOrEmpty(html))
                return null;

            var htmlDocument = GetFormattedHtmlDocument(html);

            return CreateTopic(topicUrl, htmlDocument);
        }
        private Topic CreateTopic(string topicUrl, HtmlDocument htmlDocument)
        {
            var topic = new Topic
            {
                DateCreated = DateTime.Now,
                Name = GetTopicNameFromUrl(topicUrl),
            };

            var titleNode = htmlDocument.GetElementbyId("firstHeading");
            topic.PageTitle = titleNode != null ? titleNode.InnerHtml : string.Empty;

            CreateRelatedTopics(topic, htmlDocument);

            _topicCache.SetTopic(topic.Name, topic);
            _topicCache.SetTopicHtml(topic.Name, GetHtmlFromDocument(htmlDocument));

            return topic;
        }
        private static string GetHtmlFromDocument(HtmlDocument htmlDocument)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture);
            htmlDocument.Save(writer);

            return sb.ToString();   
        }
        private void CreateRelatedTopics(Topic topic, HtmlDocument htmlDocument)
        {
            var relatedTopics = GetRelatedTopics(htmlDocument);

            //Remove current topic from list
            relatedTopics = relatedTopics.Where(x => !x.Equals(GetTopicNameFromUrl(topic.Name), StringComparison.OrdinalIgnoreCase));

            topic.RelatedTopics = relatedTopics.ToList();
        }
        private IEnumerable<string> GetRelatedTopics(HtmlDocument htmlDocument)
        {
            var relatedTopics = new List<string>();

            var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
            foreach (var link in links)
            {
                var href = link.Attributes["href"].Value;

                var topic = GetTopicNameFromUrl(href);
                //if (!string.IsNullOrEmpty(topic) && !GlobalData.IllegalTopicCharactersRegex.IsMatch(topic))
                if (!string.IsNullOrEmpty(topic))
                {
                    topic = HttpUtility.UrlDecode(topic);
                    relatedTopics.Add(topic);
                }
            }

            var distinctTopics = relatedTopics.Distinct(StringComparer.CurrentCultureIgnoreCase);
            return distinctTopics;
        }
        private string GetHtml(string url)
        {
            StreamReader rdr = null;
            HttpWebResponse resp = null;


            try
            {
                //Create request
                var req = (HttpWebRequest) WebRequest.Create(url);
                req.UserAgent = Settings.AppName;

                //Get Response
                resp = (HttpWebResponse) req.GetResponse();

                //Find out if response is compressed and if so, decompress it.
                if (resp.ContentEncoding.EqualsOrdinalIgnoreCase("gzip"))
                {
                    var bytes = resp.GetResponseStream().Decompress();
                    //var bytes = resp.GetResponseStream().ReadToEnd();
                    //var decompressedBytes = bytes.Decompress();
                    return Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    rdr = new StreamReader(resp.GetResponseStream());
                    return rdr.ReadToEnd();
                }
            }
            catch (HttpException ex)
            {
                _log.Error(url, ex);
                return string.Empty;
            }
            catch (WebException ex)
            {
                _log.Error(url, ex);
                return string.Empty;
            }
            finally
            {
                if (rdr != null)
                {
                    //rdr.Close();
                    rdr.Dispose();
                }

                if (resp != null)
                    resp.Close();
            }
        }
        private static HtmlDocument GetFormattedHtmlDocument(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            //Remove all scripts
            var scripts = doc.DocumentNode.SelectNodes("//script");
            if (scripts != null)
            {
                foreach (var script in scripts)
                {
                    script.ParentNode.RemoveChild(script);
                }
            }

            //Extract the content tag and delete everthing else.
            //We'll create everything else from scratch.
            var content = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
            CleanHtml(content);

            //var nodes = content.SelectNodes("//a[@href^='/w/index.php");


            var metaKeywords = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
            var originalTitleNode = doc.DocumentNode.SelectSingleNode("/html/head/title");
            var titleText = " - {0}".ToFormat(Settings.AppName);
            if(originalTitleNode != null)
            {
                var leftSideOfTitle = originalTitleNode.InnerHtml.Split('-')[0];
                titleText = leftSideOfTitle.Trim() + titleText;
            }

            doc.DocumentNode.RemoveAll();

            //Create the html wrapper tag
            var htmlNode = doc.CreateElement("html");
            doc.DocumentNode.AppendChild(htmlNode);

            //Create the head tag
            var headNode = doc.CreateElement("head");
            htmlNode.AppendChild(headNode);

            var titleNode = doc.CreateElement("title");
            titleNode.InnerHtml = titleText;
            headNode.AppendChild(titleNode);

            var wikiScriptNode = doc.CreateElement("script");
            #if DEBUG
            wikiScriptNode.Attributes.Append("src", "/cache/WikiJavaScriptInclude/{0}/javascript".ToFormat(DateTime.Now.ToString("MMddyymmss")));
            #else
            wikiScriptNode.Attributes.Append("src", "/cache/WikiJavaScriptInclude/022010/javascript");
            #endif
            wikiScriptNode.Attributes.Append("type", "text/javascript");
            headNode.AppendChild(wikiScriptNode);
           
            var adsenseTracking = doc.CreateElement("script");
            adsenseTracking.Attributes.Append("type", "text/javascript");
            adsenseTracking.InnerHtml = @"window.google_analytics_uacct = ""UA-8276733-2"";";
            headNode.AppendChild(adsenseTracking);

            //Add the meta tags to the head tag
            var metaRobots = doc.CreateElement("meta");
            metaRobots.Attributes.Append("name", "robots");
            metaRobots.Attributes.Append("content", "follow, index");
            headNode.AppendChild(metaRobots);
            
            if(metaKeywords != null)
                headNode.AppendChild(metaKeywords);


            //Add the stylesheet tag to the head tag
            var stylesheetLink = doc.CreateElement("link");
            stylesheetLink.Attributes.Append("rel", "stylesheet");
            
            #if DEBUG
            stylesheetLink.Attributes.Append("href", "/cache/WikiCssInclude/{0}/css".ToFormat(DateTime.Now.ToString("MMddyymmss")));
            #else
            stylesheetLink.Attributes.Append("href", "/cache/WikiCssInclude/01232011/css");
            #endif

            stylesheetLink.Attributes.Append("type", "text/css");
            stylesheetLink.Attributes.Append("media", "screen, projection");
            headNode.AppendChild(stylesheetLink);

            //Add the body tag to the html tag
            var body = doc.CreateElement("body");
            htmlNode.AppendChild(body);

            var scriptText = new StringBuilder();
            scriptText.AppendLine(@"<script type=""text/javascript""><!--");
            scriptText.AppendLine(@"google_ad_client = ""pub-2126357856738903"";");
            scriptText.AppendLine(@"///* Main Header 728x90, created 6/7/09 */");
            scriptText.AppendLine(@"google_ad_slot = ""9285973395"";");
            scriptText.AppendLine(@"google_ad_width = 728;");
            scriptText.AppendLine(@"google_ad_height = 90;");
            scriptText.AppendLine(@"//-->");
            scriptText.AppendLine(@"</script>");
            scriptText.AppendLine(@"<script type=""text/javascript""");
            scriptText.AppendLine(@"src=""http://pagead2.googlesyndication.com/pagead/show_ads.js"">");
            scriptText.AppendLine(@"</script>");

            var adDiv = doc.CreateElement("div");
            adDiv.Attributes.Append("id", "main-ads");
            adDiv.InnerHtml = scriptText.ToString();
            body.AppendChild(adDiv);

            var wikiContentDiv = doc.CreateElement("div");
            wikiContentDiv.Attributes.Append("id", "wikicontent");
            body.AppendChild(wikiContentDiv);

            wikiContentDiv.AppendChild(content);

            var tracking1 = doc.CreateElement("script");
            tracking1.Attributes.Append("type", "text/javascript");
            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(@"    var gaJsHost = ((""https:"" == document.location.protocol) ? ""https://ssl."" : ""http://www."");");
            contentBuilder.AppendLine(@"    document.write(unescape(""%3Cscript src='"" + gaJsHost + ""google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E""));");
            tracking1.InnerHtml = contentBuilder.ToString();
            body.AppendChild(tracking1);

            var tracking2 = doc.CreateElement("script");
            tracking2.Attributes.Append("type", "text/javascript");
            contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(@"    try {");
            contentBuilder.AppendLine(@"            var pageTracker = _gat._getTracker(""UA-8276733-2"");");
            contentBuilder.AppendLine(@"            pageTracker._trackPageview();");
            contentBuilder.AppendLine(@"}   catch (err) { }");
            tracking2.InnerHtml = contentBuilder.ToString();
            body.AppendChild(tracking2);

            //var allLinks = doc.DocumentNode.SelectNodes("//a");
            //if (allLinks != null)
            //{
            //    foreach (var link in allLinks)
            //    {
            //        var href = link.Attributes["href"];
            //        if (href != null)
            //        {
            //            var match = GlobalData.WikipediaLinkRegex.Match(href.Value);
            //            if(match.Success)
            //            {
                            
            //            }
                        
            //        }
            //    }
            //}

            
            //Remove all unecessary html
            CleanHtml(content);

            return doc;
        }
        private static void CleanHtml(HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Comment)
                node.ParentNode.RemoveChild(node);

            foreach (var childNode in node.ChildNodes)
            {
                CleanHtml(childNode);
            }
        } 

        /// <summary>
        /// Gets the topic portion of a url
        /// </summary>
        /// <param name="url">The url of the topic</param>
        public string GetTopicNameFromUrl(string topicUrl)
        {
            var match = RegularExpressions.TopicLinkRegex.Match(topicUrl);
            var group = match.Groups["Topic"];

            if (!group.Success)
                return string.Empty;

            return group.Value;
        }

        public bool TopicContainsRelatedTopic(string currentTopicUrl, string relatedTopicUrl)
        {
            var cur = GetTopicByUrl(currentTopicUrl);
            return cur != null && cur.RelatedTopics.Contains(relatedTopicUrl, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if a topic exists.
        /// </summary>
        /// <param name="topicUrl">The url of the topic</param>
        public bool DoesTopicExist(string topicUrl)
        {
            return GetTopicByUrl(topicUrl) != null;
        }

        public string GetTopicHtml(string name)
        {
            return _topicCache.GetTopicHtml(name);
        }
    }
}
