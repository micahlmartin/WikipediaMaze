    using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public static class Rpx
    {
        #region - Constants -
        private readonly static string API_KEY = Settings.RpxApiKey;
        private const string BASE_URL = "https://rpxnow.com";
        #endregion

        /// <summary>
        /// Gets the profile data for the user.
        /// </summary>
        /// <param name="token">Athentication token.</param>
        public static XElement GetProfileData(string token)
        {
            var query = new Dictionary<string, string> {{"token", token}};
            return ApiCall("auth_info", query);
        }

        /// <summary>
        /// Get's all mappings
        /// </summary>
        /// <param name="primaryKey">The record Id from the database for the user.</param>
        public static IList<string> Mappings(string primaryKey)
        {
            var query = new Dictionary<string, string> {{"primaryKey", primaryKey}};
            var rsp = ApiCall("mappings", query);
            var oids = (XElement)rsp.FirstNode;
            var result = new List<string>();
            foreach (var item in oids.Elements())
            {
                result.Add(item.Value);
            }
            return result;
        }

        /// <summary>
        /// Maps the OpenId of the user to the record Id from the database.
        /// </summary>
        /// <param name="identifier">The OpenId of the user</param>
        /// <param name="primaryKey">The record Id of the user from the database</param>
        public static void Map(string identifier, string primaryKey)
        {
            var query = new Dictionary<string, string> {{"identifier", identifier}, {"primaryKey", primaryKey}};
            ApiCall("map", query);
        }

        /// <summary>
        /// Unmaps the openId of the user to the record Id from the database.
        /// </summary>
        /// <param name="identifier">The OpenId of the user</param>
        /// <param name="primaryKey">The record Id of the user from the database</param>
        public static void RemoveMapping(string identifier, string primaryKey)
        {
            var query = new Dictionary<string, string> {{"identifier", identifier}, {"primaryKey", primaryKey}};
            ApiCall("unmap", query);
        }

        private static XElement ApiCall(string methodName, IDictionary<string, string> partialQuery)
        {
            var query = new Dictionary<string, string>(partialQuery) {{"format", "xml"}, {"apiKey", API_KEY}};

            var sb = new StringBuilder();
            foreach (var e in query)
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }
                sb.Append(HttpUtility.UrlEncode(e.Key, Encoding.UTF8));
                sb.Append('=');
                sb.Append(HttpUtility.UrlEncode(e.Value, Encoding.UTF8));
            }
            string data = sb.ToString();

            var url = new Uri(BASE_URL + "/api/v2/" + methodName);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            // Write the request
            var stOut = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            stOut.Write(data);
            stOut.Close();
            var response = (HttpWebResponse)request.GetResponse();
            var dataStream = response.GetResponseStream();
            var resp = XElement.Load(XmlReader.Create(dataStream));
// ReSharper disable PossibleNullReferenceException
            if (!resp.Attribute("stat").Value.Equals("ok"))
// ReSharper restore PossibleNullReferenceException
            {
                throw new InvalidOperationException("Unexpected API error");
            }
            return resp;
        }
    }
}
