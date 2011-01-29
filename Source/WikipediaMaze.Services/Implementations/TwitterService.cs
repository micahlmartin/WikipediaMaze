using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth;
using TwitterLib;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using WikipediaMaze.Core.Properties;

namespace WikipediaMaze.Services
{
    public class TwitterService : ITwitterService
    {
        #region Constants

        private const string TwitterTokenManagerKey = "TwitterTokenManager";
        private const string TwitterAccessTokenKey = "TwitterAccessToken";

        #endregion

        #region Fields

        WebConsumer _twitter;
        UrlShorteningService _shorteningService;
        IRepository _repository;
        IAuthenticationService _authenticationService;

        #endregion

        #region Constructors

        public TwitterService(IRepository repository, IAuthenticationService authenticationService)
        {
            _twitter = new WebConsumer(TwitterConsumer.ServiceDescription, TokenManager);
            _shorteningService = new TwitterLib.UrlShorteningService(ShorteningService.Bitly);
            _repository = repository;
            _authenticationService = authenticationService;
        }

        #endregion

        private TokenManager TokenManager
        {
            get
            {
                var tokenManager = (TokenManager)HttpRuntime.Cache[TwitterTokenManagerKey];
                if (tokenManager == null)
                {
                    tokenManager = new TokenManager(Settings.Default.TwitterConsumerKey, Settings.Default.TwitterConsumerSecret);
                    HttpRuntime.Cache.Add(TwitterTokenManagerKey, tokenManager, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }

                return tokenManager;
            }
        }
        private string AccessToken
        {
            get { return (string)HttpContext.Current.Session[TwitterAccessTokenKey]; }
            set { HttpContext.Current.Session[TwitterAccessTokenKey] = value; }
        }

        public void RequestAuthorization()
        {
            _twitter = new WebConsumer(TwitterConsumer.ServiceDescription, TokenManager);
            var accessTokenResponse = _twitter.ProcessUserAuthorization();
            if (accessTokenResponse != null)
                AccessToken = accessTokenResponse.AccessToken;
            else
                _twitter.Channel.Send(_twitter.PrepareRequestUserAuthorization()); 
        }
        public void TweetPuzzle(int puzzleId)
        {
            //Can't tweet unless OAuth has occurred.
            if (!IsAuthorized) throw new UnauthorizedAccessException();

            Puzzle puzzle;

            using (_repository.OpenSession())
            {
                puzzle = _repository.All<Puzzle>().ById(puzzleId);
            }

            if (puzzle == null) return;

            var message = "Check out this new puzzle http://www.wikipediamaze.com/puzzles/{0} {1} to {2}. Can you solve it? #wikipediamaze".ToFormat(puzzleId, puzzle.StartTopic.FormatTopic(), puzzle.EndTopic.FormatTopic());
            message = _shorteningService.ShrinkUrls(message);
            UpdateStatus(message);
        }
        public void TweetSolution(int solutionId)
        {
            //Can't tweet unless OAuth has occurred.
            if (!IsAuthorized) throw new UnauthorizedAccessException();

            SolutionProfile solution;

            using (_repository.OpenSession())
            {
                solution = _repository.All<SolutionProfile>().Where(x => x.Id == solutionId).SingleOrDefault();
            }

            if (solution == null) return;

            //Can't tweet solutions from other users.
            if (_authenticationService.CurrentUserId != solution.UserId) throw new UnauthorizedAccessException();

            var message = "Solved the puzzle http://www.wikipediamaze.com/puzzles/{0} {1} to {2} in {3} steps. Can you beat it? #wikipediamaze".ToFormat(solution.PuzzleId, solution.StartTopic.FormatTopic(), solution.EndTopic.FormatTopic(), solution.StepCount);
            message = _shorteningService.ShrinkUrls(message);
            UpdateStatus(message);
        }
        private void UpdateStatus(string status)
        {
            var doc = TwitterConsumer.UpdateStatus(_twitter, AccessToken, status);
        }
        public bool IsAuthorized
        {
            get
            {
                return !string.IsNullOrEmpty(AccessToken); 
            }
        }
    }
}