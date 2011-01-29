using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace WikipediaMaze.Services
{
    public class TokenManager : IConsumerTokenManager
    {
        #region Fields

        private Dictionary<string, string> _tokensAndSecrets;

        #endregion

        #region Constructors

        public TokenManager(string consumerKey, string consumerSecret)
        {
            _tokensAndSecrets = new Dictionary<string, string>();
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        #endregion

        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            _tokensAndSecrets.Remove(requestToken);
            _tokensAndSecrets[accessToken] = accessTokenSecret;
        }

        public string GetTokenSecret(string token)
        {
            return _tokensAndSecrets[token];
        }

        public TokenType GetTokenType(string token)
        {
            throw new NotImplementedException();
        }

        public bool IsRequestTokenAuthorized(string requestToken)
        {
            throw new NotImplementedException();
        }

        public void StoreNewRequestToken(DotNetOpenAuth.OAuth.Messages.UnauthorizedTokenRequest request, DotNetOpenAuth.OAuth.Messages.ITokenSecretContainingMessage response)
        {
            _tokensAndSecrets[response.Token] = response.TokenSecret;
        }
    }
}