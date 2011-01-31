using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using WikipediaMaze.Core;
using System.Web.Caching;
using log4net;

namespace WikipediaMaze.Services
{
    public class TopicCache : ITopicCache
    {
        #region Constants

        private const string BASE_TOPIC_CACHE_KEY = "Topic/";

	    #endregion

        #region Fields

        private ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    	#endregion
               
        #region ITopicCache Members

        public string GetTopicHtml(string name)
        {
            var formattedName = name.EncodeAsFileName();
            _log.Info("Retriveing Topic File {0}".ToFormat(formattedName));

            if (File.Exists(BaseFilePath + formattedName))
            {
                using (var rdr = new StreamReader(BaseFilePath + formattedName))
                {
                    _log.Info("Reading Topic File {0}".ToFormat(formattedName));
                    return rdr.ReadToEnd();
                }
            }

            return string.Empty;
        }

        public void SetTopicHtml(string name, string html)
        {
            var formattedName = name.EncodeAsFileName();
            using (var sw = new StreamWriter(BaseFilePath + formattedName))
            {
                _log.Info("Writing Topic File {0}".ToFormat(formattedName));
                sw.Write(html);
            }
        }

        public bool TopicExists(string name)
        {
            _log.Info("Topic {0} exists? {1}".ToFormat(name, File.Exists(BaseFilePath + name)));
            return File.Exists(BaseFilePath + name);
        }
        
        public Topic GetTopic(string name)
        {
            return HttpContext.Current.Cache[BASE_TOPIC_CACHE_KEY + name] as Topic;
        }

        public void SetTopic(string name, Topic topic)
        {
            if (topic == null)
                HttpContext.Current.Cache.Remove(BASE_TOPIC_CACHE_KEY + name);
            else
                HttpContext.Current.Cache.Add(BASE_TOPIC_CACHE_KEY + name, topic, null, Cache.NoAbsoluteExpiration, new TimeSpan(Settings.TopicExpirationDays, 0, 0, 0), CacheItemPriority.High, null);
        }

        #endregion

        private static string BaseFilePath
        {
            get { return HttpContext.Current.Request.MapPath("/App_Data/Topics/"); }
        }

    }
}
