using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using System.Xml.Linq;
using System.Xml;
using System.Net;

namespace WikipediaMaze.Services
{
    /// <summary>
    /// A consumer capable of communicating with Twitter.
    /// </summary>
    public static class TwitterConsumer
    {
        /// <summary>
        /// The description of Twitter's OAuth protocol URIs.
        /// </summary>
        public static readonly ServiceProviderDescription ServiceDescription = new ServiceProviderDescription
        {
            RequestTokenEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/request_token", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
            UserAuthorizationEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/authorize", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
            AccessTokenEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/access_token", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
            TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
        };

        /// <summary>
        /// The URI to get a user's favorites.
        /// </summary>
        private static readonly MessageReceivingEndpoint GetFavoritesEndpoint = new MessageReceivingEndpoint("http://twitter.com/favorites.xml", HttpDeliveryMethods.GetRequest);

        /// <summary>
        /// The URI to get the data on the user's home page.
        /// </summary>
        private static readonly MessageReceivingEndpoint GetFriendTimelineStatusEndpoint = new MessageReceivingEndpoint("http://twitter.com/statuses/friends_timeline.xml", HttpDeliveryMethods.GetRequest);

        /// <summary>
        /// The URI to post a status update.
        /// </summary>
        private static readonly MessageReceivingEndpoint UpdateStatusEndpoint = new MessageReceivingEndpoint("http://twitter.com/statuses/update.xml", HttpDeliveryMethods.PostRequest);

        public static XDocument GetUpdates(ConsumerBase twitter, string accessToken)
        {
            IncomingWebResponse response = twitter.PrepareAuthorizedRequestAndSend(GetFriendTimelineStatusEndpoint, accessToken);
            return XDocument.Load(XmlReader.Create(response.GetResponseReader()));
        }
        public static XDocument UpdateStatus(ConsumerBase twitter, string accessToken, string message)
        {
            var data = new Dictionary<string, string>();
            data.Add("status", message);

            HttpWebRequest request = twitter.PrepareAuthorizedRequest(UpdateStatusEndpoint, accessToken, data);

            var response = twitter.Channel.WebRequestHandler.GetResponse(request);
            return XDocument.Load(XmlReader.Create(response.GetResponseReader()));
        }
        public static XDocument GetFavorites(ConsumerBase twitter, string accessToken)
        {
            IncomingWebResponse response = twitter.PrepareAuthorizedRequestAndSend(GetFavoritesEndpoint, accessToken);
            return XDocument.Load(XmlReader.Create(response.GetResponseReader()));
        }
    }
}